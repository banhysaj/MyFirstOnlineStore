
using API.DTO;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class OrderController: BaseApiController
    {
        private readonly StoreContext _context;
        public OrderController(StoreContext context){
            _context = context;
        }

        [HttpPost("addOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == orderDto.userId);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var shoppingCart = await _context.Shopping_Carts.FirstOrDefaultAsync(sc => sc.Id == orderDto.shoppingCartId);
            if (shoppingCart == null)
            {
                return BadRequest("Shopping cart not found");
            }

            var order = new Order
            {
                User = user,
                ShoppingCart = shoppingCart,
                TotalPrice = orderDto.TotalPrice,
                Status = orderDto.Status,
                Address = orderDto.Address,
                OrderDate = DateTime.Now
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(orderDto);
        }
        
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<OrderDto>>> GetOrdersByUser(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.ShoppingCart)
                .Where(o => o.User.Id == userId)
                .ToListAsync();

            var orderDtos = orders.Select(o => new OrderDto
            {
                userId = o.User.Id,
                TotalPrice = o.TotalPrice,
                Status = o.Status,
                Address = o.Address,
                shoppingCartId = o.ShoppingCart.Id
            }).ToList();

            return Ok(orderDtos);
        }

    }
}