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
                return View("Login");
            }
        }
        
        public IActionResult Index()
        {
            PreserveViewData();
            return View("Index");
        }

        [HttpGet("[action]")]
        [Route("/Login")]
        public IActionResult ToLogin()
        {
            HttpContext.Session.Clear();
            return View("Login");
        }

        [HttpGet("[action]")]
        [Route("/Createacc")]
        public IActionResult ToCreateacc()
        {
            HttpContext.Session.Clear();
            return View("Createacc");
        }

        [HttpGet("[action]")]
        [Route("/Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }

        [HttpGet("[action]")]
        [Route("/Dash")]
        public IActionResult Dashboard()
        {
            if (!HasSession())
            {
                return Logout();
            }
            PreserveViewData();
            return View("Dashboard");
        }


        [HttpGet("[action]")]
        [Route("/Project")]
        public IActionResult Project()
        {
            if (!HasSession())
            {
                return Logout();
            }
            PreserveViewData();
            return View("Project");
        }

        [HttpGet("[action]")]
        [Route("/Ticket")]
        public IActionResult Ticket()
        {
            if (!HasSession())
            {
                return Logout();
            }
            PreserveViewData();
            return View("Ticket");
        }

        [HttpGet("[action]")]
        [Route("/Profile")]
        public IActionResult Profile()
        {
            if (!HasSession())
            {
                return Logout();
            }
            PreserveViewData();
            return View("Profile");
        }

        private void PreserveViewData()
        {
            ViewData["username"] = HttpContext.Session.GetString("username");
        }

        private Boolean HasSession()
        {
            if (HttpContext.Session.GetString("username") is null)
            {
                return false;
            }
            return true;
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