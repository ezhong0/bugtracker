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
using System.Data;

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

                string sql = "SELECT * FROM USER WHERE email = '" + createaccModel.Email + "'";
                using MySqlConnection conn = new(CONN_STR);
                conn.Open();


                DataTable ds = new();
                using (MySqlDataAdapter da = new())
                {
                    da.SelectCommand = new MySqlCommand(sql, conn);
                    da.Fill(ds);

                }

                HttpContext.Session.SetInt32("userid", (int)((ds.Rows[0])["userid"]));


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
            Random random = new();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            int codelength = 6;

            string tempcode, sql;

            while(true)
            {
                tempcode = new string(Enumerable.Repeat(chars, codelength).Select(s => s[random.Next(s.Length)]).ToArray());
                sql = "SELECT * FROM PROJECT WHERE joincode = '" + tempcode + "'";
                DataTable ds = new();
                using (MySqlConnection conn0 = new(CONN_STR))
                {
                    conn0.Open();
                    using (MySqlDataAdapter da = new())
                    {
                        da.SelectCommand = new MySqlCommand(sql, conn0);
                        da.Fill(ds);
                    }
                }
                if(ds.Rows.Count == 0)
                {
                    break;
                }
            }

            sql = "INSERT INTO PROJECT (title, description, datemodified, joincode, UserId) VALUES " +
               "('" + dashboardModel.Title + "', '" + dashboardModel.Description + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + tempcode + "', " + HttpContext.Session.GetInt32("userid") + ")";

            using MySqlConnection conn = new(CONN_STR);
            conn.Open();
            MySqlCommand com = new(sql, conn);
            com.ExecuteNonQuery();

            dashboardModel.JoinCode = tempcode;

            return JoinProject(dashboardModel);
        }

        [HttpGet("[action]")]
        [Route("/JoinProject")]
        public IActionResult JoinProject(DashboardModel dashboardModel)
        {
            string sql = "SELECT * FROM PROJECT WHERE joincode = '" + dashboardModel.JoinCode + "'";
            using MySqlConnection conn = new(CONN_STR);
            conn.Open();


            DataTable ds = new();
            using (MySqlDataAdapter da = new())
            {
                da.SelectCommand = new MySqlCommand(sql, conn);
                da.Fill(ds);

            }
            if (ds.Rows.Count == 0)
            {
                return RedirectToAction("Dashboard", "Home");
            }

            sql = "INSERT INTO PROJECTUSERJUNCTION (projectid, userid) VALUES (" + (int)((ds.Rows[0])["projectid"]) + ", " + HttpContext.Session.GetInt32("userid") + ")";
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
            return RedirectToAction("Home", "Home");
        }

        [HttpGet("[action]")]
        [Route("/Dash")]
        public IActionResult Dashboard()
        {
            if (!HasSession())
            {
                return Logout();
            }

            LoadProjectTable();
            
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

            LoadProjectTable();

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

        private void LoadProjectTable()
        {
            string sql = "SELECT * FROM PROJECTUSERJUNCTION J INNER JOIN PROJECT P ON P.projectid = J.projectid WHERE J.userid = " + HttpContext.Session.GetInt32("userid");

            //string sql = "SELECT * FROM PROJECT WHERE USERID = '" + HttpContext.Session.GetInt32("userid") + "'";

            DataTable ds = new();
            using (MySqlConnection conn = new(CONN_STR))
            {
                conn.Open();
                using MySqlDataAdapter da = new();
                da.SelectCommand = new MySqlCommand(sql, conn);
                da.Fill(ds);
            }
			
			ds.DefaultView.Sort = "title desc";

            ViewBag.projects = (from DataRow dr in ds.Rows
                                select new DashboardModel()
                                {
                                    ProjectId = Convert.ToInt32(dr["projectid"]),
                                    Title = dr["title"].ToString(),
                                    Description = dr["description"].ToString(),
                                    DateModified = dr["datemodified"].ToString(),
                                    JoinCode = dr["joincode"].ToString(),
                                    UserId = Convert.ToInt32(dr["userid"])

                                }).ToList();

            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            //var students = from s in db.Students
            //               select s;
            //switch (sortOrder)
            //{
            //    case "name_desc":
            //        students = students.OrderByDescending(s => s.LastName);
            //        break;
            //    case "Date":
            //        students = students.OrderBy(s => s.EnrollmentDate);
            //        break;
            //    case "date_desc":
            //        students = students.OrderByDescending(s => s.EnrollmentDate);
            //        break;
            //    default:
            //        students = students.OrderBy(s => s.LastName);
            //        break;
            //}

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