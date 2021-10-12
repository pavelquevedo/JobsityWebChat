using Microsoft.AspNet.SignalR;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebChat.Api.Models;
using WebChat.Utils.Common.Models.Request;
using WebChat.Utils.Interface;
using WebChat.Utils.Services;
using State = WebChat.Utils.Common.Enum.State;

namespace WebChat.Api
{
    /// <summary>
    /// Class which centralizes the SignalR 
    /// communication between the views and the api
    /// </summary>
    public class ChatHub : Hub
    {
        private readonly IQueueService _queueService;
        private WebChatDBEntities _db;
        private const string stockQuoteRegEx = "(?i)^(/STOCK)[=].{1,15}";

        public ChatHub()
        {
            _queueService = new StockQueueProducer();
            _db = new WebChatDBEntities();
        }

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
        public void Send(int roomId, string userName, int userId, string message)
        {

            string messageDate = DateTime.Now.ToString();

            //Check if user sent a stock request command
            if (Regex.IsMatch(message.ToUpper(), stockQuoteRegEx))
            {
                //Add request to the service bus queue
                _queueService.CreateStockQueue(new StockQuoteRequest()
                {
                    RoomId = roomId,
                    UserId = userId,
                    Command = message
                });
            }
            else
            {
                //Insert message to the database
                var newMessage = new Message()
                {
                    RoomID = roomId,
                    CreationDate = DateTime.Now,
                    UserID = userId,
                    Message1 = message,
                    StateID = (int)State.ACTIVE

                };

                _db.Message.Add(newMessage);
                _db.SaveChanges();

                //Send message to a specific group
                Clients.Group(roomId.ToString()).sendChat(userName, message, messageDate, userId);
            }
        }
    }
}