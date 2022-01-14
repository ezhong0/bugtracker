using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BugTracker.Models;
using BugTracker.Security;
using BugTracker.Views.Models;

namespace BugTracker.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("[action]")]
        [Route("/Success")]
        public IActionResult Success(LoginModel loginModel)
        {
            LoginResult loginResult = Authenticate.AttemptLogin(loginModel.User, loginModel.Password);

            if (loginResult.Success)
            {
                return View();
            }
            else
            {
                return Index();
            }
        }

        public IActionResult Index()
        {
            return View();
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