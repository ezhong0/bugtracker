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
using MySql.Data.MySqlClient;


namespace BugTracker.Controllers
{
    public class HomeController : Controller
    {
        static readonly string CONN_STR = "server=localhost;database=bugtrackerdb;user=root;password=Bl@ckpink";

        [HttpGet("[action]")]
        [Route("/AttemptLogin")]
        public IActionResult AttemptLogin(LoginModel loginModel)
        {
            LoginResult loginResult = Authenticate.AttemptLogin(loginModel.Email, loginModel.Password);

            if (loginResult.Success)
            {
                HttpContext.Session.SetString("firstname", loginResult.FirstName);
                HttpContext.Session.SetString("lastname", loginResult.LastName);
                HttpContext.Session.SetString("email", loginResult.Email);
                HttpContext.Session.SetInt32("userid", loginResult.UserId);


                ViewData["fullname"] = HttpContext.Session.GetString("firstname") + " " + HttpContext.Session.GetString("lastname");
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                return RedirectToAction("ToLogin", "Home");
            }
        }

        [HttpGet("[action]")]
        [Route("/AccountCreate")]
        public IActionResult CreateAccount(CreateaccModel createaccModel)
        {

            if (Authenticate.CreateAccount(createaccModel.Email, createaccModel.FirstName, createaccModel.LastName, createaccModel.Password))
            {
                HttpContext.Session.SetString("firstname", createaccModel.FirstName);
                HttpContext.Session.SetString("lastname", createaccModel.LastName);
                HttpContext.Session.SetString("email", createaccModel.Email);

                ViewData["fullname"] = HttpContext.Session.GetString("firstname") + " " + HttpContext.Session.GetString("lastname");
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                return RedirectToAction("ToCreateacc", "Home");
            }
        }

        [HttpGet("[action]")]
        [Route("/CreateProject")]
        public IActionResult CreateProject(DashboardModel dashboardModel)
        {
            string sql = "INSERT INTO PROJECT (title, description, datemodified, UserId) VALUES ('" + dashboardModel.Title + "', '" + dashboardModel.Description + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', " + HttpContext.Session.GetInt32("userid") + ")";

            using MySqlConnection conn = new(CONN_STR);
            conn.Open();
            MySqlCommand com = new(sql, conn);
            com.ExecuteNonQuery();

            return RedirectToAction("Dashboard", "Home");
        }

        public IActionResult Index()
        {
            PreserveViewData();
            return View("Index");
        }

        [HttpGet("[action]")]
        [Route("/Home")]
        public IActionResult Home()
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
            ViewData["fullname"] = HttpContext.Session.GetString("firstname") + " " + HttpContext.Session.GetString("lastname");
        }

        private Boolean HasSession()
        {
            if (HttpContext.Session.GetString("email") is null)
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