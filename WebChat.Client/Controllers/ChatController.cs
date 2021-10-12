using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebChat.Utils.Common.Models.Response;
using WebChat.Utils.Tools;

namespace WebChat.Client.Controllers
{
    public class ChatController : BaseController
    {
        /// <summary>
        /// Gets all the messages in an specific chat room 
        /// </summary>
        /// <param name="roomId">Chat room requested</param>
        /// <returns>If chatroom exists, gets user's messages and returns messaging panel view</returns>
        public ActionResult Messages(int roomId)
        {
            if (UserSession == null)
            {
                return RedirectToAction("Login", "Home");
            }
            //Getting current room info
            ApiResponse currentRoomResponse =
                    RequestUtil.ExecuteWebMethod<RoomResponse>(string.Format("api/rooms/{0}", roomId),
                    RestSharp.Method.GET, UserSession.AccessToken);

            //Getting messages from the current room
            ApiResponse messageResponses =
                    RequestUtil.ExecuteWebMethod<List<MessageResponse>>(
                        string.Format("api/messages/{0}/{1}", roomId, UserSession.Id),
                        RestSharp.Method.GET, UserSession.AccessToken);

            if (currentRoomResponse.StatusCode == HttpStatusCode.OK && messageResponses.StatusCode == HttpStatusCode.OK)
            {
                //Passing room information to viewbag
                ViewBag.CurrentRoom = currentRoomResponse.Content;
                //Passing messages information to view
                return View(messageResponses.Content);
            }

            return View();
        }
    }
}