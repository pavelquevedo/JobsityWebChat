using System.Web.Mvc;
using WebChat.Client.Models.ViewModels;
using WebChat.Utils.Common.Models.Request;
using WebChat.Utils.Common.Models.Response;
using WebChat.Utils.Tools;

namespace WebChat.Client.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            LoginRequest model = new LoginRequest();
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Password = Encrypt.GetSHA256(model.Password).ToUpper();

            UserResponse userResponse =
                RequestUtil.ExecuteWebMethod<UserResponse>("api/authentication/authenticate", RestSharp.Method.POST, string.Empty, model);

            if (userResponse != null)
            {
                Session["User"] = userResponse;
                return RedirectToAction("Index", "Lobby");
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

            UserResponse userResponse =
            RequestUtil.ExecuteWebMethod<UserResponse>("api/user/register", RestSharp.Method.POST, string.Empty, model);

            if (userResponse != null)
            {
                Session["User"] = userResponse;
            }

            return View();
        }
    }
}