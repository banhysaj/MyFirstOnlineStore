using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> AddOrder([FromBody] OrderDto orderDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == orderDto.userId);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var shoppingCart = await _context.Shopping_Carts.FirstOrDefaultAsync(sc => sc.User.Id == orderDto.userId);
            var order = new Order
            {
                User = user,
                OrderDate = DateTime.Now,
                TotalPrice = orderDto.TotalPrice,
                Status = orderDto.Status,
                Address = orderDto.Address,
                ShoppingCart = shoppingCart
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(order);
        }

    }
}