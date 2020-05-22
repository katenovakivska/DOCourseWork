using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseWork;
using CourseWork.Models;
using System.Security.Cryptography.X509Certificates;

namespace CourseWork.Controllers
{
    //[Produces("application/json")]
    //[Route("~/OrderProducts")]
    public class OrderProductsController : Controller
    {
        OrderContext _context;

        public OrderProductsController(OrderContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create(int orderId)
        {
            ViewBag.Id = orderId;
            var products = _context.Products;
            //return Ok(products);
            return View(products);
        }

        [HttpPost]
        public IActionResult Create(OrderProduct orderProduct)
        {
            if (orderProduct.ProductId == 0 || orderProduct.OrderId == 0)
            {
                return RedirectToAction(nameof(Create));
            }
            double k = (double)orderProduct.WriteOffSum / orderProduct.Amount;
            orderProduct.Price = /*(int)Math.Round(k)*/Convert.ToInt32(k);
            double m = (double)orderProduct.Amount * orderProduct.Price;
            orderProduct.Sum = /*(int)Math.Round(m)*/Convert.ToInt32(m);
            orderProduct.Order = _context.Orders.FirstOrDefault(o => o.OrderId == orderProduct.OrderId);
            orderProduct.Product = _context.Products.FirstOrDefault(p => p.ProductId == orderProduct.ProductId);
            _context.OrderProducts.Add(orderProduct);
            var order = _context.Orders.FirstOrDefault(x => x.OrderId == orderProduct.OrderId);
            order.AllWriteOffSum = order.AllWriteOffSum + orderProduct.WriteOffSum;
            order.AllSum = order.AllSum + (orderProduct.Amount * orderProduct.Price);
            order.Disbalance = Math.Abs(order.AllWriteOffSum - order.AllSum);
            _context.Orders.Update(order);
            _context.SaveChanges();
            
            return RedirectToAction("Create", "OrderProducts", new { orderId = orderProduct.OrderId });
        }

        [HttpGet]
        public IActionResult Delete(int orderId, int productId)
        {
            var orderProduct = _context.OrderProducts.FirstOrDefault(x => x.OrderId == orderId && x.ProductId == productId);
            var order = _context.Orders.FirstOrDefault(x => x.OrderId == orderId);
            order.AllWriteOffSum = order.AllWriteOffSum - orderProduct.WriteOffSum;
            order.AllSum = order.AllSum - orderProduct.Sum;
            order.Disbalance = Math.Abs(order.AllWriteOffSum - order.AllSum);
            _context.Orders.Update(order);
            int id = orderId;
            _context.OrderProducts.Remove(orderProduct);
            _context.SaveChanges();

            return RedirectToAction("Details", "Orders", new { orderId = id });
        }

    }
}
