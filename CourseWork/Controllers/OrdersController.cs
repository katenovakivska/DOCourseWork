using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseWork;
using CourseWork.Models;
using CourseWork.Methods;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace CourseWork.Controllers
{
    public class OrdersController : Controller
    {
        OrderContext _context;

        public OrdersController(OrderContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var orders = _context.Orders.ToList();
            return View(orders);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Order order)
        {
            if (order.Customer == null || order.PhoneNumber == null)
            {
                return RedirectToAction(nameof(Create));
            }
            order.Date = DateTime.Now;
            order.AllSum = 0;
            order.Disbalance = 0;
            order.AllWriteOffSum = 0;
            _context.Orders.Add(order);
            _context.SaveChanges();
            return RedirectToAction("Create", "OrderProducts", new { orderId = order.OrderId });
        }

        [HttpGet]
        public IActionResult Delete(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(x => x.OrderId == orderId);
            if (order == null)
            {
                return RedirectToAction(nameof(GetAll));
            }
            var orderProducts = _context.OrderProducts.Where(x => x.OrderId == order.OrderId);
            foreach (var op in orderProducts)
            {
                _context.OrderProducts.Remove(op);
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();
            return RedirectToAction("GetAll", "Orders");
        }

        [HttpGet]
        public IActionResult Update(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(x => x.OrderId == orderId);
            if (order == null)
            {
                return RedirectToAction(nameof(GetAll));
            }
            return View(order);
        }

        [HttpGet]
        public IActionResult Change(int orderId, string customer, string phone)
        {
            var order = _context.Orders.FirstOrDefault(x => x.OrderId == orderId);
            if (order == null)
            {
                return RedirectToAction(nameof(GetAll));
            }
            order.Customer = customer;
            order.PhoneNumber = phone;

            _context.Orders.Update(order);
            _context.SaveChanges();

            return RedirectToAction("GetAll", "Orders");
        }
        [HttpGet]
        public IActionResult Details(int orderId)
        {
            FrankWolf frank = new FrankWolf();
            if (orderId == 0)
            {
                return NotFound();
            }

            var order = _context.Orders.FirstOrDefault(x => x.OrderId == orderId);
            var orderProducts = _context.OrderProducts.Where(x => x.OrderId == orderId);
            foreach (var p in orderProducts)
            {
                p.Product = _context.Products.FirstOrDefault(x => x.ProductId == p.ProductId);
            
            }
            ViewBag.Order = order;
            int disbalance = 0;
            List<OrderProduct> changed = new List<OrderProduct>(); 
            if(orderProducts.Count() != 0 || orderProducts.Count() != 1)
            {
                var xNew = frank.FrankWolfMethod(order, orderProducts, disbalance).ToList();
                ViewBag.Disbalance = disbalance;
                changed = BetterPrices(changed, orderProducts, xNew);
                
                ViewBag.Products = changed;
                var sum = 0;var writeOff = 0;
                foreach(var op in changed)
                {
                    sum += op.Sum;
                    writeOff += op.WriteOffSum;
                }
                ViewBag.Sum = sum;
                ViewBag.AllSum = writeOff;
            }
          
            return View(orderProducts);
        }

        public List<OrderProduct> BetterPrices(List<OrderProduct> changed, IQueryable<OrderProduct> orderProducts, List<int> xNew)
        {
            int index = 0;
            foreach (var op in orderProducts)
            {
                OrderProduct orderProduct = new OrderProduct();
                orderProduct.Amount = op.Amount;
                orderProduct.Product = op.Product;
                orderProduct.Price = xNew[index];
                orderProduct.Sum = orderProduct.Price * orderProduct.Amount;
                orderProduct.WriteOffSum = op.WriteOffSum;
                changed.Add(orderProduct);
                index++;
            }

            return changed;
        }
       
        [HttpGet]
        public IActionResult Random()
        {
            FrankWolf frank = new FrankWolf();
            Order order = new Order();
            Product product = new Product();
            OrderProduct orderProduct = new OrderProduct();
            List<OrderProduct> orderProducts = new List<OrderProduct>();
            Random random = new Random();
            int count = random.Next(1, 5);
            order = GenerateOrder(order);
            for (int i = 0; i < count; i++)
            {
                product = new Product();
                orderProduct = new OrderProduct();
                product = GenerateProduct(product);
                orderProduct = GenerateOrderProducts(order.OrderId, product.ProductId, orderProduct);
                orderProducts.Add(orderProduct);
            }
            
            foreach (var p in orderProducts)
            {
                p.Product = _context.Products.FirstOrDefault(x => x.ProductId == p.ProductId);
            }
            ViewBag.Order = order;
            int disbalance = 0;
            List<OrderProduct> changed = new List<OrderProduct>();
            if (orderProducts.Count() != 0)
            {
                var xNew = frank.FrankWolfMethod(order, orderProducts, disbalance).ToList();
                ViewBag.Disbalance = disbalance;
                changed = BetterPrices(changed, orderProducts, xNew);

                ViewBag.Products = changed;
                var sum = 0; var writeOff = 0;
                foreach (var op in changed)
                {
                    sum += op.Sum;
                    writeOff += op.WriteOffSum;
                }
                ViewBag.Sum = sum;
                ViewBag.AllSum = writeOff;
            }
            _context.SaveChanges();
            return View(orderProducts);
        }

        public Order GenerateOrder(Order order)
        {
            Random random = new Random();
            const string chars = "0123456789";
            order.Date = DateTime.Now;
            order.Customer = "Customer";
            order.PhoneNumber = new string(Enumerable.Repeat(chars, 12).Select(s => s[random.Next(s.Length)]).ToArray());

            _context.Orders.Add(order);
            _context.SaveChanges();

            return order;
        }

        public Product GenerateProduct(Product product)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            product.Name = new string(Enumerable.Repeat(chars, 9).Select(s => s[random.Next(s.Length)]).ToArray());
            product.Description = "Description";
            _context.Products.Add(product);
            _context.SaveChanges();

            return product;
        }
        public OrderProduct GenerateOrderProducts(int orderId, int productId, OrderProduct orderProduct)
        {
            Random random = new Random();

            orderProduct.OrderId = orderId;
            orderProduct.ProductId = productId;
            orderProduct.WriteOffSum = random.Next(1000, 20000);
            orderProduct.Amount = random.Next(2, 15);
            double k = orderProduct.WriteOffSum / orderProduct.Amount;
            orderProduct.Price = (int)Math.Round(k);
            double m = orderProduct.Amount * orderProduct.Price;
            orderProduct.Sum = (int)Math.Round(m);
            _context.OrderProducts.Add(orderProduct);
            _context.SaveChanges();

            var order = _context.Orders.FirstOrDefault(x => x.OrderId == orderId);
            order.AllWriteOffSum = order.AllWriteOffSum + orderProduct.WriteOffSum;
            order.AllSum = order.AllSum + (orderProduct.Amount * orderProduct.Price);
            order.Disbalance = Math.Abs(order.AllWriteOffSum - order.AllSum);
            _context.Orders.Update(order);
            _context.SaveChanges();

            return orderProduct;
        }
        public List<OrderProduct> BetterPrices(List<OrderProduct> changed, List<OrderProduct> orderProducts, List<int> xNew)
        {
            int index = 0;
            foreach (var op in orderProducts)
            {
                OrderProduct orderProduct = new OrderProduct();
                orderProduct.Amount = op.Amount;
                orderProduct.Product = op.Product;
                orderProduct.Price = xNew[index];
                orderProduct.Sum = orderProduct.Price * orderProduct.Amount;
                orderProduct.WriteOffSum = op.WriteOffSum;
                changed.Add(orderProduct);
                index++;
            }

            return changed;
        }
        [HttpGet]
        public IActionResult ChooseFile()
        {
            FileReading fileReading = new FileReading();
            var files = fileReading.AllFiles();
            ViewBag.Files = files;
                
            return View();
        }
        [HttpGet]
        public IActionResult File(string fileName)
        {
            FileReading fileReading = new FileReading();
            fileReading.FileRead(fileName);
            FrankWolf frank = new FrankWolf();
            Order order = new Order();
            Product product = new Product();
            OrderProduct orderProduct = new OrderProduct();
            List<OrderProduct> orderProducts = new List<OrderProduct>();
            
            int count = fileReading.count;
            //order = GenerateOrder(order);
            order = fileReading.FileRead(fileName);
            order = AddOrder(order);
            
            //orderProducts = order.OrdersProducts;
            //for (int i = 0; i < count; i++)
            foreach (var op in order.OrdersProducts)
            {
                product = new Product();
                orderProduct = new OrderProduct();
                //product = GenerateProduct(product);
                //product = AddProduct(op.Product);
                orderProduct.Amount =op.Amount /*fileReading.num[0, i]*/;
                orderProduct.WriteOffSum = op.WriteOffSum /*fileReading.num[1, i]*/;
                orderProduct = FileOrderProducts(order.OrderId, op.Product.ProductId,/*product.ProductId,*/ orderProduct);
                orderProducts.Add(orderProduct);
            }

            foreach (var p in orderProducts)
            {
                p.Product = _context.Products.FirstOrDefault(x => x.ProductId == p.ProductId);
            }
            ViewBag.Order = order;
            int disbalance = 0;
            List<OrderProduct> changed = new List<OrderProduct>();
            if (orderProducts.Count() != 0)
            {
                var xNew = frank.FrankWolfMethod(order, orderProducts, disbalance).ToList();
                ViewBag.Disbalance = disbalance;
                changed = BetterPrices(changed, orderProducts, xNew);

                ViewBag.Products = changed;
                var sum = 0; var writeOff = 0;
                foreach (var op in changed)
                {
                    sum += op.Sum;
                    writeOff += op.WriteOffSum;
                }
                ViewBag.Sum = sum;
                ViewBag.AllSum = writeOff;
            }
            _context.SaveChanges();
            return View(orderProducts);
        }
        public Order AddOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();

            return order;
        
        }
        public Product AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();

            return product;
        }
        public OrderProduct FileOrderProducts(int orderId, int productId, OrderProduct orderProduct)
        {
            Random random = new Random();

            orderProduct.OrderId = orderId;
            orderProduct.ProductId = productId;
            double k = orderProduct.WriteOffSum / orderProduct.Amount;
            orderProduct.Price = (int)Math.Round(k);
            double m = orderProduct.Amount * orderProduct.Price;
            orderProduct.Sum = (int)Math.Round(m);
            _context.OrderProducts.Add(orderProduct);
            _context.SaveChanges();

            var order = _context.Orders.FirstOrDefault(x => x.OrderId == orderId);
            order.AllWriteOffSum = order.AllWriteOffSum + orderProduct.WriteOffSum;
            order.AllSum = order.AllSum + (orderProduct.Amount * orderProduct.Price);
            order.Disbalance = Math.Abs(order.AllWriteOffSum - order.AllSum);
            _context.Orders.Update(order);
            _context.SaveChanges();

            return orderProduct;
        }
    }
}
