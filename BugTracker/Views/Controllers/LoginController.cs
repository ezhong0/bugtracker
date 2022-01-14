//using System.Net;
//using System.Net.Http;
//using BugTracker.Models;
//using BugTracker.Security;
//using BugTracker.Views.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace BugTracker.Controllers
//{
//    public class LoginController : Controller
//    {
//        [HttpGet("[action]")]
//        [Route("/Auth")]
//        public ActionResult Auth(LoginModel loginModel)
//        {
//            LoginResult loginResult = Authenticate.AttemptLogin(loginModel.User, loginModel.Password);

//            if (loginResult.Success)
//            {
//                return View();
//            }
//            else
//            {
//                return Failure();
//            }
//        }

//        [HttpGet("[action]")]
//        [Route("/Index")]
//        public ActionResult Failure()
//        {
//            return View();
//        }
//    }
//}