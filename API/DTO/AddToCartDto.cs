using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class AddToCartDto
    {
    [Required]
    public int ProductId { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public int Quantity { get; set; }
    }
}