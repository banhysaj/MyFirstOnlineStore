
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class User: BaseEntity
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Password { get; set; }
        
        public string Token { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}