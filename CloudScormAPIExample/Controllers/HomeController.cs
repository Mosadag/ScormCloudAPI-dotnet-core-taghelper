using CloudScormAPIExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CloudScormAPIExample.Services;
using Com.RusticiSoftware.Cloud.V2.Api;
using Com.RusticiSoftware.Cloud.V2.Model;
using Microsoft.AspNetCore.Http;

namespace CloudScormAPIExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ScormServices _services;
        public HomeController(ILogger<HomeController> logger, ScormServices services)
        {
            _logger = logger;
            _services = services;
        }

        public IActionResult Index()
        {
            ViewBag.AllCourse = _services.LoadAllCourse();

            return View();

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
