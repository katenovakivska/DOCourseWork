using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.Models
{
    public class OrderProduct
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public int OldPrice { get; set; }
        public int NewPrice { get; set; }
        public int OldSum { get; set; }
        public int NewSum { get; set; }
        public int WriteOffSum { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
