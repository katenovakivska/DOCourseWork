using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseWork;
using CourseWork.Models;
using System.Data;

namespace CourseWork.Controllers
{
    //[Produces("application/json")]
    //[Route("~/Products")]
    public class ProductsController : Controller
    {
        OrderContext _context;

        public ProductsController(OrderContext context)
        {
            _context = context;
        } 

        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _context.Products.ToList();
            //return Ok(products);
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (product.Name == null || product.Description == null)
            {
                return RedirectToAction(nameof(Create));
            }
            _context.Products.Add(product);
            _context.SaveChanges();
            return View(product);
        }
    
        [HttpGet]
        public IActionResult Delete(int productId)
        {
            var product = _context.Products.FirstOrDefault(x => x.ProductId == productId);
            if (product == null)
            {
                return RedirectToAction(nameof(GetAll));
            }
            var orderProducts = _context.OrderProducts.Where(x => x.ProductId == product.ProductId);
            foreach (var op in orderProducts)
            {
                _context.OrderProducts.Remove(op);
            }

            _context.Products.Remove(product);
            _context.SaveChanges();
            
            return RedirectToAction("GetAll", "Products");
        }

        [HttpGet]
        public IActionResult Update(int productId)
        {
            var product = _context.Products.FirstOrDefault(x => x.ProductId == productId);
            if (product == null)
            {
                return RedirectToAction(nameof(GetAll));
            }
            //ViewBag.Product = product;
            return View(product);
        }

        [HttpGet]
        public IActionResult Change(int productId, string productName, string description)
        {
            var product = _context.Products.FirstOrDefault(x => x.ProductId == productId);
            if (product == null)
            {
                return RedirectToAction(nameof(GetAll));
            }
            product.Name = productName;
            product.Description = description;

            _context.Products.Update(product);
            _context.SaveChanges();

            return RedirectToAction("GetAll", "Products");
        }
    }
}
