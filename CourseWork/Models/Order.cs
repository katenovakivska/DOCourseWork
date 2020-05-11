using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime Date { get; set; }
        public string Customer { get; set; }
        public string PhoneNumber { get; set; }
        public virtual ICollection<OrderProduct> OrdersProducts { get; set; }
    }
}
