using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CourseWork.Models;

namespace CourseWork.Controllers
{
    public class HomeController : Controller
    {
        ////OrderContext db;
        public HomeController(/*OrderContext context*/)
        {
            //db = context;
        }
        public IActionResult Index()
        {
            return View(/*db.Products.ToList()*/);
        }
    }
}
