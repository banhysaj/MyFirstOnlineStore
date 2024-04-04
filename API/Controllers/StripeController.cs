using API.DTO;
using API.Helpers;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using Core.Entities;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeController : ControllerBase
    {
        private readonly IOptions<StripeSettings> _stripeSettings;
        private readonly StoreContext _context;

        public StripeController(IOptions<StripeSettings> stripeSettings, StoreContext context)
        {
            _stripeSettings = stripeSettings;
            _context = context;
        }

        [HttpPost("create-payment-intent")]
        public ActionResult CreatePaymentIntent([FromBody] PaymentIntentDto paymentIntentDto)
        {
            StripeConfiguration.ApiKey = _stripeSettings.Value.SecretKey;

            var options = new PaymentIntentCreateOptions
            {
                Amount = paymentIntentDto.Amount,
                Currency = "usd",
                Metadata = new Dictionary<string, string>
                {
                    { "userId", paymentIntentDto.UserId.ToString() }
                }
            };

            var service = new PaymentIntentService();
            PaymentIntent paymentIntent = service.Create(options);

            return Ok(new { clientSecret = paymentIntent.ClientSecret });
        }
        
        [HttpPost("payment-completed")]
    public async Task<IActionResult> PaymentCompleted([FromBody] Event stripeEvent)
    {
        if (stripeEvent.Type == "payment_intent.succeeded")
        {
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            string userId = paymentIntent.Metadata["userId"];

            Console.WriteLine($"UserId: {userId}"); // Log statement

            // Retrieve the user from your database using the userId
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(userId));

            if (user == null)
            {
                Console.WriteLine("User not found"); // Log statement
                return NotFound("User not found");
            }

            // Retrieve the shopping cart for the user
            var shoppingCart = await _context.Shopping_Carts.FirstOrDefaultAsync(sc => sc.User.Id == user.Id);

            if (shoppingCart == null)
            {
                Console.WriteLine("Shopping cart not found"); // Log statement
                return BadRequest("Shopping cart not found");
            }

            // Create a new order
            var order = new Order
            {
                User = user,
                ShoppingCart = shoppingCart,
                TotalPrice = paymentIntent.Amount / 100,
                Status = "Completed",
                Address = user.Address,
                OrderDate = DateTime.Now
            };

            _context.Orders.Add(order);

            try
            {
                await _context.SaveChangesAsync();
                Console.WriteLine("Order saved to database"); // Log statement
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving order to database: {ex.Message}"); // Log statement
                throw;
            }

            return Ok();
        }

        return BadRequest();
    }
    }
}