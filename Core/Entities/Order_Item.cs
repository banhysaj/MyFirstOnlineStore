using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Order_Item: BaseEntity
{
    [Required]
    public int Quantity { get; set; }
    [Required]
    public Product Product {get; set;}
    [Required]
    public Order Order { get; set; }
}