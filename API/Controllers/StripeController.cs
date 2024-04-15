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
        
    }
}