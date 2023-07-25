using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkProject.Entities
{
    public class Customer
    {
        public Customer()
        {
            Orders = new List<Order>();
        }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int ShoppingScore { get; set; }
        public string PhoneNumber { get; set; }

        public List<Order> Orders { get; set; }
    }
}
