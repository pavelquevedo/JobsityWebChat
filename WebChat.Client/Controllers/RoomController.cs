using System.Collections.Generic;
using System.Net;
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
            ApiResponse roomResponseList =
                RequestUtil.ExecuteWebMethod<List<RoomResponse>>("api/rooms", RestSharp.Method.GET, UserSession.AccessToken, null);

            if (roomResponseList.StatusCode == HttpStatusCode.OK)
            {
                return View(roomResponseList.Content);
            }
            else if (roomResponseList.StatusCode == HttpStatusCode.NoContent)
            {
                ViewBag.error = "No rooms available, come back later.";
                return View();
            }

            ViewBag.error = "An error ocurred, please try again later.";
            return View();
        }
    }
}