using Microsoft.VisualBasic;
namespace API.DTO
{
    public class OrderDto
    {
        public int userId { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public int? shoppingCartId { get; set; }

    }
}