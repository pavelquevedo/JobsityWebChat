using System.Net;
using System.Web.Mvc;
using WebChat.Client.Models.ViewModels;
using WebChat.Utils.Common.Models.Request;
using WebChat.Utils.Common.Models.Response;
using WebChat.Utils.Tools;

namespace WebChat.Client.Controllers
{
    /// <summary>
    /// Controller to manage the login and user register
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Loads an empty LoginRequest model
        /// </summary>
        /// <returns>Login view</returns>
        [HttpGet]
        public ActionResult Login()
        {
            LoginRequest model = new LoginRequest();
            return View(model);
        }

        /// <summary>
        /// Performs user authentication
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Password = Encrypt.GetSHA256(model.Password);

            //Authenticating user
            ApiResponse authUserResponse =
                RequestUtil.ExecuteWebMethod<UserResponse>("api/accounts/authenticate", RestSharp.Method.POST, string.Empty, model);

            //If user exists, access to the chatroom
            if (authUserResponse.StatusCode == HttpStatusCode.OK)
            {
                Session["User"] = authUserResponse.Content;
                return RedirectToAction("Index", "Room");

            }else if (authUserResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.error = "Incorrect credentials";
            }
            
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            return View(model);
        }

        /// <summary>
        /// Registers a new user into the system
        /// </summary>
        /// <param name="model">Model with user's properties</param>
        /// <returns>If registration is successful, it redirects to Rooms menu</returns>
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Encrypt model password
            model.Password = Encrypt.GetSHA256(model.Password);

            //Performing user registration
            ApiResponse userResponse = RequestUtil
                .ExecuteWebMethod<UserResponse>("api/users", RestSharp.Method.POST, string.Empty, model);

            if (userResponse.StatusCode == HttpStatusCode.OK)
            {
                Session["User"] = userResponse.Content;
                return RedirectToAction("Index", "Room");
            }
            else if(userResponse.StatusCode == HttpStatusCode.Conflict)
            {
                ModelState.AddModelError("Login", "Seems like someone took that username before.");
            }

            return View(model);

        }
    }
}