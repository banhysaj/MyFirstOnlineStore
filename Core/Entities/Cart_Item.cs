
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Cart_Item: BaseEntity
    {
        [Required]
        public int Quantity { get; set; }
        
        public Product Product {get; set;}
        public ShoppingCart ShoppingCart { get; set; }
    }
}