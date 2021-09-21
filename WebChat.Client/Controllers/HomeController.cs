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
            UserResponse userResponse =
                RequestUtil.ExecuteWebMethod<UserResponse>("api/authentication/authenticate", RestSharp.Method.POST, string.Empty, model);

            //If user exists, access to the chatroom
            if (userResponse != null)
            {
                Session["User"] = userResponse;
                return RedirectToAction("Index", "Room");
            }

            ViewBag.error = "Incorrect credentials";
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            return View(model);
        }

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
            UserResponse userResponse =
            RequestUtil.ExecuteWebMethod<UserResponse>("api/user/register", RestSharp.Method.POST, string.Empty, model);

            if (userResponse != null)
            {
                Session["User"] = userResponse;
                return RedirectToAction("Index", "Room");
            }
            else
            {
                ModelState.AddModelError("Login", "Seems like someone took that username before.");
                return View(model);
            }
        }
    }
}