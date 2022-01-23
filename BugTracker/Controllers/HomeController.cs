using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using BugTracker.Models;
using BugTracker.Security;
using BugTracker.Views.Models;

namespace BugTracker.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("[action]")]
        [Route("/Dashboard")]
        public IActionResult Login(LoginModel loginModel)
        {
            LoginResult loginResult = Authenticate.AttemptLogin(loginModel.User, loginModel.Password);

            if (loginResult.Success)
            {
                HttpContext.Session.SetString("username", loginModel.User);
                ViewData["username"] = HttpContext.Session.GetString("username");
                return View("Dashboard");
            }
            else
            {
                return View("Index");
            }
        }

        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpGet("[action]")]
        [Route("/Dash")]
        public IActionResult Dashboard()
        {
            ViewData["username"] = HttpContext.Session.GetString("username");
            return View("Dashboard");
        }

        public IActionResult Project()
        {
            return View("Project");
        }

        public IActionResult Ticket()
        {
            return View("Ticket");
        }

        public IActionResult Profile()
        {
            return View("Profile");
        }

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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