using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace Core.Entities
{
    public class Order: BaseEntity
    {
        public decimal TotalPrice { get; set; }
        public string  Status { get; set; }
        public DateTime OrderDate { get; set; }
        public User User { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public string Address {get;set;}
        
        public Order(){
            OrderDate = DateTime.Now;
        }
    }
}