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
        private IEmployeeRepository _employeeRepository;

        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public IActionResult Index()
        {
            //if((Request.ContentType) == "application/json")
            //{
            //    return Json(_employeeRepository.GetEmployee(1).Name);
            //}

            //return View(_employeeRepository.GetEmployee(1));
            return View(_employeeRepository.GetEmployees());
        }


        //public IActionResult Index()
        //{
        //    return View(_employeeRepository.GetEmployees());
        //}
    }
}
