
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class ShoppingCart: BaseEntity
    {
        [Required]
        public User User { get; set; }
        public ICollection<Cart_Item> Cart_Items { get; set; }
        
    }
}