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
        public int OldAllSum { get; set; }
        public int NewAllSum { get; set; }
        public int AllWriteOffSum { get; set; }
        public int OldDisbalance { get; set; }
        public int NewDisbalance { get; set; }
        public virtual ICollection<OrderProduct> OrdersProducts { get; set; }
    }
}
