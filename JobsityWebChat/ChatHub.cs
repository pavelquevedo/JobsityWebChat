using Microsoft.AspNet.SignalR;
using System;
using System.Threading.Tasks;
using WebChat.Api.Models;

namespace WebChat.Api
{
    public class ChatHub : Hub
    {
        /// <summary>
        /// Overriding method OnConnected to notify when an user enters
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            Clients.All.enterUser();
            return base.OnConnected();
        }

        /// <summary>
        /// Method used to join an user to a room
        /// </summary>
        /// <param name="roomId">Room Id</param>
        public void JoinRoom(int roomId)
        {
            Groups.Add(Context.ConnectionId, roomId.ToString());
        }

        /// <summary>
        /// Send message to a specific room 
        /// </summary>
        /// <param name="roomId">Room unique identifier</param>
        /// <param name="userName">User name to show</param>
        /// <param name="userId">User unique identifier</param>
        /// <param name="message">Message body</param>
        /// <param name="accessToken"></param>
        public void Send(int roomId, string userName, int userId, string message, string accessToken)
        {
            string messageDate = DateTime.Now.ToString();

            using (WebChatDBEntities db = new WebChatDBEntities())
            {
                var newMessage = new Message();
                newMessage.RoomID = roomId;
                newMessage.CreationDate = DateTime.Now;
                newMessage.UserID = userId;
                newMessage.Message1 = message;
                newMessage.StateID = 1;

                db.Message.Add(newMessage);
                db.SaveChanges();
            }

            //Send message to everyone
            //Clients.All.sendChat(userName, message, messageDate, userId);

            //Send message to a specific group
            Clients.Group(roomId.ToString()).sendChat(userName, message, messageDate, userId);

        }
    }
}