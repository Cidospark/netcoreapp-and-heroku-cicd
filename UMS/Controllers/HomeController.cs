using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UMS.Models;

namespace UMS.Controllers
{
    public class HomeController : Controller
    {
        
        public HomeController()
        {
        }
        public IActionResult Index(string Username)
        {
            ViewBag.Nmae = Username;
            return View();
        }

    }
}
