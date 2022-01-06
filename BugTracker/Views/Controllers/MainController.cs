/*

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BugTracker.Models;

namespace BugTracker.Views.Controllers
{
    public class MainController : Controller
    {
        private readonly ILogger<MainController> _logger;

        public MainController(ILogger<MainController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Index()
        {
            UserViewModel model = null;

            if (Request.HasFormContentType)
            {
                if (Request.Form.ContainsKey("userId"))
                {
                    var id = Convert.ToInt32(Request.Form["userId"]);
                    model = new UserViewModel(id);
                    TempData["EmpId"] = id;
                }
            }
            else if (TempData.ContainsKey("EmpId"))
            {
                model = new UserViewModel((int)TempData["EmpId"]);
            }

            if (model == null)
            {
                return RedirectToAction("Login");
            }
            return View(model);
        }

        public IActionResult Insights()
        {
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

*/