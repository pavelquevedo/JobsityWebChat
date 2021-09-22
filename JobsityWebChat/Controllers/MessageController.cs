using Microsoft.AspNet.SignalR;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using WebChat.Utils.Common.Enum;
using WebChat.Utils.Common.Models.Response;

namespace WebChat.Api.Controllers
{    /// <summary>
     /// This controller manages the room's messages
     /// </summary>
    [RoutePrefix("api/message")]
    public class MessageController : BaseController
    {

        /// <summary>
        /// Web method to send messages to the chathub
        /// </summary>
        /// <param name="roomId">Room unique identifier</param>
        /// <param name="model">Stooq.com api csv response converted into StockQuoteResponse object</param>
        /// <returns></returns>
        [HttpPost]
        [Route("sendBotMessage")]
        public async Task<IHttpActionResult> SendBotMessage([FromUri] int roomId, [FromBody] StockQuoteResponse model)
        {
            //Instantiate ChatHub context
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            //Build bot response with csv data
            var botResponse = string.Format("{0} quote is ${1} per share", model.Symbol, model.High);
            
            try
            {
                //Send message to specified RoomId
                hubContext.Clients.Group(roomId.ToString()).sendChat("Chat Bot", botResponse, DateTime.Now.ToString(), 0);//TODO: Change bot user id
                model.State = State.SENT;

                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Web method to retrieve last 50 messages from an specific room
        /// </summary>
        /// <param name="roomId">Room unique identifier</param>
        /// <param name="userId">User unique identifier</param>
        /// <returns></returns>
        [HttpGet]
        [Route("getRoomMessages")]
        public async Task<IHttpActionResult> GetRoomMessages([FromUri] int roomId, int userId)
        {
            try
            {
                //Getting active messages from provided roomId, selecting just 50 messages
                var messagesQuery = await (from m in dbContext.Message
                                           where m.StateID == (int)State.ACTIVE
                                           && m.RoomID == roomId
                                           orderby m.CreationDate descending
                                           select new MessageResponse
                                           {
                                               Id = m.Id,
                                               UserID = m.UserID,
                                               Message = m.Message1,
                                               CreationDate = m.CreationDate,
                                               UserFullName = m.User.FirstName + " " + m.User.LastName,
                                               MessageType = m.UserID == userId ? MessageType.OWN : MessageType.ALIEN
                                           }).Take(50).ToListAsync();

                if (messagesQuery != null)
                {
                    return Ok(messagesQuery);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
