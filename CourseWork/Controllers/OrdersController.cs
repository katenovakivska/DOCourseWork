using CourseWork.Methods;
using CourseWork.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseWork.Controllers
{
    //[Produces("application/json")]
    //[Route("~/Orders")]
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

            List<OrderProduct> changedFrank = new List<OrderProduct>();
            List<OrderProduct> changedAnnealing = new List<OrderProduct>();
            List<OrderProduct> changedGenetic = new List<OrderProduct>();
            if (orderProducts.Count() != 0 || orderProducts.Count() != 1)
            {
                int count = orderProducts.Count();
                var xNew = frank.FrankWolfMethod(order, orderProducts).ToList();

                changedFrank = BetterPrices(changedFrank, orderProducts.ToList(), xNew);

                int[] q = new int[count];
                int[] s = new int[count];
                int i = 0;
                foreach (var op in orderProducts)
                {
                    s[i] = op.WriteOffSum;
                    q[i] = op.Amount;
                    i++;
                }
                Methods.Task task = new Methods.Task(q, s, order.AllWriteOffSum);
                SimulatedAnnealing simulatedAnnealing = new SimulatedAnnealing(task, 200, 100);
                TaskPair pair = simulatedAnnealing.executeAlgorithm();
                changedAnnealing = BetterPrices(changedAnnealing, orderProducts.ToList(), pair.X.ToList());

                Genetic genetic = new Genetic(task);
                TaskPair pairGenetic = genetic.ExecuteAlgorithm();
                changedGenetic = BetterPrices(changedGenetic, orderProducts.ToList(), pairGenetic.X.ToList());

                ViewBag.Products = changedFrank;
                ViewBag.ProductsAnnealing = changedAnnealing;
                ViewBag.ProductsGenetic = changedGenetic;
                var sum = 0; var writeOff = 0;
                foreach (var op in changedFrank)
                {
                    sum += op.Sum;
                    writeOff += op.WriteOffSum;
                }
                ViewBag.Sum = sum;
                ViewBag.AllSum = writeOff;
                ViewBag.Disbalance = Math.Abs(sum - writeOff);

                var sumAnnealing = 0; var writeOffAnnealing = 0;
                foreach (var op in changedAnnealing)
                {
                    sumAnnealing += op.Sum;
                    writeOffAnnealing += op.WriteOffSum;
                }
                ViewBag.SumAnnealing = sumAnnealing;
                ViewBag.AllSumAnnealing = writeOffAnnealing;
                ViewBag.DisbalanceAnnealing = Math.Abs(sumAnnealing - writeOffAnnealing);

                var sumGenetic = 0; var writeOffGenetic = 0;
                foreach (var op in changedGenetic)
                {
                    sumGenetic += op.Sum;
                    writeOffGenetic += op.WriteOffSum;
                }
                ViewBag.SumGenetic = sumGenetic;
                ViewBag.AllSumGenetic = writeOffGenetic;
                ViewBag.DisbalanceGenetic = Math.Abs(sumGenetic - writeOffGenetic);
            }

            return View(orderProducts);
        }

        [HttpGet]
        public IActionResult Random(int Amount, int leftSum, int rightSum, int leftAmount, int rightAmount)
        {
            FrankWolf frank = new FrankWolf();
            Random random = new Random();
            if (leftSum > rightSum || leftAmount > rightAmount || leftSum < 0 || rightAmount < 0 || rightAmount < 0 || leftAmount < 0)
            {
                return RedirectToAction(nameof(ChooseAmount));
            }
            Order order = RandomHandler.GetRandomizedOrder(Amount, leftSum, rightSum, leftAmount, rightAmount);

            ViewBag.Order = order;

            List<OrderProduct> changedFrank = new List<OrderProduct>();
            List<OrderProduct> changedAnnealing = new List<OrderProduct>();
            List<OrderProduct> changedGenetic = new List<OrderProduct>();
            if (order.OrdersProducts.Count() != 0)
            {

                var xNew = frank.FrankWolfMethod(order, order.OrdersProducts).ToList();

                changedFrank = BetterPrices(changedFrank, order.OrdersProducts.ToList(), xNew);
                int[] q = new int[Amount];
                int[] s = new int[Amount];
                int i = 0;
                foreach (var op in order.OrdersProducts)
                {
                    s[i] = op.WriteOffSum;
                    q[i] = op.Amount;
                    i++;
                }
                Methods.Task task = new Methods.Task(q, s, order.AllWriteOffSum);
                SimulatedAnnealing simulatedAnnealing = new SimulatedAnnealing(task, 200, 100);
                TaskPair pair = simulatedAnnealing.executeAlgorithm();
                changedAnnealing = BetterPrices(changedAnnealing, order.OrdersProducts.ToList(), pair.X.ToList());

                Genetic genetic = new Genetic(task);
                TaskPair pairGenetic = genetic.ExecuteAlgorithm();
                changedGenetic = BetterPrices(changedGenetic, order.OrdersProducts.ToList(), pairGenetic.X.ToList());

                ViewBag.ProductsFrank = changedFrank;
                ViewBag.ProductsAnnealing = changedAnnealing;
                ViewBag.ProductsGenetic = changedGenetic;

                var sumFrank = 0; var writeOffFrank = 0;
                foreach (var op in changedFrank)
                {
                    sumFrank += op.Sum;
                    writeOffFrank += op.WriteOffSum;
                }
                ViewBag.SumFrank = sumFrank;
                ViewBag.AllSumFrank = writeOffFrank;
                ViewBag.DisbalanceFrank = Math.Abs(sumFrank - writeOffFrank);
                var sumAnnealing = 0; var writeOffAnnealing = 0;
                foreach (var op in changedAnnealing)
                {
                    sumAnnealing += op.Sum;
                    writeOffAnnealing += op.WriteOffSum;
                }
                ViewBag.SumAnnealing = sumAnnealing;
                ViewBag.AllSumAnnealing = writeOffAnnealing;
                ViewBag.DisbalanceAnnealing = Math.Abs(sumAnnealing - writeOffAnnealing);

                var sumGenetic = 0; var writeOffGenetic = 0;
                foreach (var op in changedGenetic)
                {
                    sumGenetic += op.Sum;
                    writeOffGenetic += op.WriteOffSum;
                }
                ViewBag.SumGenetic = sumGenetic;
                ViewBag.AllSumGenetic = writeOffGenetic;
                ViewBag.DisbalanceGenetic = Math.Abs(sumGenetic - writeOffGenetic);
            }
            _context.SaveChanges();
            return View(order.OrdersProducts);
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
            FrankWolf frank = new FrankWolf();
            Product product = new Product();
            List<OrderProduct> changedFrank = new List<OrderProduct>();
            List<OrderProduct> changedAnnealing = new List<OrderProduct>();
            List<OrderProduct> changedGenetic = new List<OrderProduct>();

            Order order = fileReading.FileRead(fileName);
            int count = order.OrdersProducts.Count;

            order.AllSum = 0;

            if (order.OrdersProducts.Count() != 0)
            {

                foreach (var orderProduct in order.OrdersProducts)
                {
                    order.AllWriteOffSum = order.AllWriteOffSum + orderProduct.WriteOffSum;
                    orderProduct.Price = Convert.ToInt32((double)orderProduct.WriteOffSum / orderProduct.Amount);
                    orderProduct.Sum = Convert.ToInt32((double)orderProduct.Price * orderProduct.Amount);
                    order.AllSum = order.AllSum + (orderProduct.Amount * orderProduct.Price);
                    order.Disbalance = Math.Abs(order.AllWriteOffSum - order.AllSum);

                }

                ViewBag.Order = order;
                var xNew = frank.FrankWolfMethod(order, order.OrdersProducts).ToList();

                changedFrank = BetterPrices(changedFrank, order.OrdersProducts.ToList(), xNew);
                int[] q = new int[count];
                int[] s = new int[count];
                int i = 0;
                foreach (var op in order.OrdersProducts)
                {
                    s[i] = op.WriteOffSum;
                    q[i] = op.Amount;
                    i++;
                }
                Methods.Task task = new Methods.Task(q, s, order.AllWriteOffSum);
                SimulatedAnnealing simulatedAnnealing = new SimulatedAnnealing(task, 200, 100);
                TaskPair pair = simulatedAnnealing.executeAlgorithm();
                changedAnnealing = BetterPrices(changedAnnealing, order.OrdersProducts.ToList(), pair.X.ToList());

                Genetic genetic = new Genetic(task);
                TaskPair pairGenetic = genetic.ExecuteAlgorithm();
                changedGenetic = BetterPrices(changedGenetic, order.OrdersProducts.ToList(), pairGenetic.X.ToList());

                ViewBag.ProductsFrank = changedFrank;
                ViewBag.ProductsAnnealing = changedAnnealing;
                ViewBag.ProductsGenetic = changedGenetic;

                var sumFrank = 0; var writeOffFrank = 0;
                foreach (var op in changedFrank)
                {
                    sumFrank += op.Sum;
                    writeOffFrank += op.WriteOffSum;
                }
                ViewBag.SumFrank = sumFrank;
                ViewBag.AllSumFrank = writeOffFrank;
                ViewBag.DisbalanceFrank = Math.Abs(sumFrank - writeOffFrank);
                var sumAnnealing = 0; var writeOffAnnealing = 0;
                foreach (var op in changedAnnealing)
                {
                    sumAnnealing += op.Sum;
                    writeOffAnnealing += op.WriteOffSum;
                }
                ViewBag.SumAnnealing = sumAnnealing;
                ViewBag.AllSumAnnealing = writeOffAnnealing;
                ViewBag.DisbalanceAnnealing = Math.Abs(sumAnnealing - writeOffAnnealing);

                var sumGenetic = 0; var writeOffGenetic = 0;
                foreach (var op in changedGenetic)
                {
                    sumGenetic += op.Sum;
                    writeOffGenetic += op.WriteOffSum;
                }
                ViewBag.SumGenetic = sumGenetic;
                ViewBag.AllSumGenetic = writeOffGenetic;
                ViewBag.DisbalanceGenetic = Math.Abs(sumGenetic - writeOffGenetic);
            }
            _context.SaveChanges();
            return View(order.OrdersProducts);
        }
        [HttpGet]
        public IActionResult ChooseAmount()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ChooseCounter()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ShowCounter(int iterations, int amount, int leftSum, int rightSum, int leftAmount, int rightAmount)
        {
            AlgorithmsRuntimeCounter counter = new AlgorithmsRuntimeCounter();
            if (leftSum > rightSum || leftAmount > rightAmount || leftSum < 0 || rightAmount < 0 || rightAmount < 0 || leftAmount < 0)
            {
                return RedirectToAction(nameof(ChooseAmount));
            }
            ViewBag.Counter = counter.CountTime(iterations, amount, leftSum, rightSum, leftAmount, rightAmount);

            return View();
        }
    }
}
