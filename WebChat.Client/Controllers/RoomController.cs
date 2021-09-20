using System.Collections.Generic;
using System.Web.Mvc;
using WebChat.Utils.Common.Models.Response;
using WebChat.Utils.Tools;

namespace WebChat.Client.Controllers
{
    /// <summary>
    /// This controller manages the chat rooms lobby
    /// </summary>
    public class RoomController : BaseController
    {
        /// <summary>
        /// Returns index view, gets available chatrooms and shows them to the client
        /// </summary>
        /// <returns>Index view</returns>
        public ActionResult Index()
        {
            if (UserSession == null)
            {
                return RedirectToAction("Login", "Home");
            }

            //Getting available chatrooms
            List<RoomResponse> roomResponseList =
                RequestUtil.ExecuteWebMethod<List<RoomResponse>>("api/room/getAll", RestSharp.Method.GET, UserSession.AccessToken, null);

            if (roomResponseList != null)
            {
                return View(roomResponseList);
            }

            ViewBag.error = "An error ocurred, please try again later.";
            return View();
        }
    }
}