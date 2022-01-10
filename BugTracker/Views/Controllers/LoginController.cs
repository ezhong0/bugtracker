//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using BugTracker.Models;
//using BugTracker.Security;
//using BugTracker.Views.Models;

//namespace BugTracker.Controllers
//{
//    public class LoginController : ApiController
//    {
//        [HttpPost]
//        public IHttpActionResult Post(LoginModel loginModel)
//        {
//            LoginResult loginResult = Authenticate.AttemptLogin(Request.GetOwinContext(), loginModel.User, loginModel.Password);

//            if (loginResult.Success)
//            {
//                return Ok();
//            }
//            else
//            {
//                return StatusCode(HttpStatusCode.Forbidden);
//            }
//        }
//    }
//}