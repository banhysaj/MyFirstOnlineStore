using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace API.DTO
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public ProductDto Product { get; set; }
    }
}