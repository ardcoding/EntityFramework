using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkProject.Entities
{
    public class Order
    {
        public string OrderId   { get; set; }
        public string CustomerId { get; set; }
        public DateTime OrderDate   { get; set; }
        public Customer Customer { get; set; }
    }
}
