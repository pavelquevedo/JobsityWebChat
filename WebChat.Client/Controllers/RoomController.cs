using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebChat.Utils.Common.Models.Response;
using WebChat.Utils.Tools;

namespace WebChat.Client.Controllers
{
    public class RoomController : BaseController
    {
        public ActionResult Index()
        {
            if (UserSession != null)
            {

                List<RoomResponse> roomResponseList =
                    RequestUtil.ExecuteWebMethod<List<RoomResponse>>("api/room/getAll", RestSharp.Method.GET, UserSession.AccessToken, null);
                if (roomResponseList != null)
                {
                    return View(roomResponseList);
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }
    }
}