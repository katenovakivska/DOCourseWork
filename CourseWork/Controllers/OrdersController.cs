using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseWork;
using CourseWork.Models;

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
            order.OldAllSum = 0;
            order.NewAllSum = 0;
            order.NewDisbalance = 0;
            order.AllWriteOffSum = 0;
            _context.Orders.Add(order);
            _context.SaveChanges();
            return RedirectToAction("Create", "OrderProducts", new { orderId = order.OrderId });
        }

        [HttpGet]
        public IActionResult Details(int orderId)
        {
            if (orderId == 0)
            {
                return NotFound();
            }

            var order = _context.Orders.FirstOrDefault(x => x.OrderId == orderId);
            var orderProducts = _context.OrderProducts.Where(x => x.OrderId == orderId);
            foreach (var op in orderProducts)
            {
                op.Product = _context.Products.FirstOrDefault(x => x.ProductId == op.ProductId);
            
            }
            ViewBag.Order = order;

            return View(orderProducts);
        }
        

      

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,Date,Customer,PhoneNumber,AllSum,AllWriteOffSum")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
