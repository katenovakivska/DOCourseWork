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

            return View(products);
        }

        [HttpPost]
        public IActionResult Create(OrderProduct orderProduct)
        {
            if (orderProduct.ProductId == 0 || orderProduct.OrderId == 0)
            {
                return RedirectToAction(nameof(Create));
            }
            double k = orderProduct.WriteOffSum / orderProduct.Amount;
            orderProduct.OldPrice = (int) Math.Round(k);
            orderProduct.NewPrice = 0;
            double m = orderProduct.Amount * orderProduct.OldPrice;
            orderProduct.OldSum = (int) Math.Round(m);
            orderProduct.NewSum = 0;
            orderProduct.Order = _context.Orders.FirstOrDefault(o => o.OrderId == orderProduct.OrderId);
            orderProduct.Product = _context.Products.FirstOrDefault(p => p.ProductId == orderProduct.ProductId);
            _context.OrderProducts.Add(orderProduct);
            var order = _context.Orders.FirstOrDefault(x => x.OrderId == orderProduct.OrderId);
            order.AllWriteOffSum = order.AllWriteOffSum + orderProduct.WriteOffSum;
            order.OldAllSum = order.OldAllSum + (orderProduct.Amount * orderProduct.OldPrice);
            order.OldDisbalance = Math.Abs(order.AllWriteOffSum - order.OldAllSum);
            _context.Orders.Update(order);
            _context.SaveChanges();

            return RedirectToAction("Create", "OrderProducts", new { orderId = orderProduct.OrderId });
        }

        [HttpGet]
        public IActionResult Delete(int orderId, int productId)
        {
            var orderProduct = _context.OrderProducts.FirstOrDefault(x => x.OrderId == orderId && x.ProductId == productId);
            int id = orderId;
            _context.OrderProducts.Remove(orderProduct);
            _context.SaveChanges();

            return RedirectToAction("Details", "Orders", new { orderId = id });
        }
        // GET: OrderProducts
        public async Task<IActionResult> Index()
        {
            var orderContext = _context.OrderProducts.Include(o => o.Order).Include(o => o.Product);
            return View(await orderContext.ToListAsync());
        }

        // GET: OrderProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderProduct = await _context.OrderProducts
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orderProduct == null)
            {
                return NotFound();
            }

            return View(orderProduct);
        }

        // GET: OrderProducts/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
            return View();
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderProduct = await _context.OrderProducts.FindAsync(id);
            if (orderProduct == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderProduct.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", orderProduct.ProductId);
            return View(orderProduct);
        }

        // POST: OrderProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,ProductId,Amount,OldPrice,NewPrice,OldSum,NewSum,WriteOffSum")] OrderProduct orderProduct)
        {
            if (id != orderProduct.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderProductExists(orderProduct.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderProduct.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", orderProduct.ProductId);
            return View(orderProduct);
        }

       

        private bool OrderProductExists(int id)
        {
            return _context.OrderProducts.Any(e => e.OrderId == id);
        }
    }
}
