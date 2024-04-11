using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Order: BaseEntity
    {
        [Required]
        public decimal TotalPrice { get; set; }
        [Required]
        public string  Status { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public ICollection<Order_Item> Order_Items { get; set; }
        [Required]
        public string Address {get;set;}
        
        public Order(){
            OrderDate = DateTime.Now;
        }
    }
}