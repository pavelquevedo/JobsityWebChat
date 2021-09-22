using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebChat.Utils.Common.Models.Response;
using WebChat.Utils.Tools;

namespace WebChat.Client.Controllers
{
    public class ChatController : BaseController
    {
        public ActionResult Messages(int roomId)
        {
            if (UserSession == null)
            {
                return RedirectToAction("Login", "Home");
            }
            //Getting current room info
            RoomResponse currentRoom =
                    RequestUtil.ExecuteWebMethod<RoomResponse>(string.Format("api/room/getSingle?roomId={0}", roomId),
                    RestSharp.Method.GET, UserSession.AccessToken);

            //Getting messages from the current room
            List<MessageResponse> messageResponses =
                    RequestUtil.ExecuteWebMethod<List<MessageResponse>>(
                        string.Format("api/message/getRoomMessages?roomId={0}&userId={1}", roomId, UserSession.Id),
                        RestSharp.Method.GET, UserSession.AccessToken);

            if (currentRoom != null)
            {
                //Passing room information to viewbag
                ViewBag.CurrentRoom = currentRoom;
            }
            //Sort list
            if (messageResponses.Count > 0)
            {
                messageResponses = messageResponses.OrderBy(m => m.CreationDate).ToList();
            }

            return View(messageResponses);
        }
    }
}