using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using WebChat.Utils.Common.Enum;
using WebChat.Utils.Common.Models.Response;

namespace WebChat.Api.Controllers
{
    /// <summary>
    /// This controller manages rooms information
    /// </summary>
    [RoutePrefix("api/rooms")]
    public class RoomController : BaseController
    {
        /// <summary>
        /// Web method to retrieve a single room
        /// </summary>
        /// <returns>Single room object</returns>
        [HttpGet]
        [Route("{roomId:int}")]
        public async Task<IHttpActionResult> GetSingle(int roomId)
        {
            try
            {
                //Getting single room
                var roomQuery = await (from r in dbContext.Room
                                        where r.Id == roomId
                                        select new RoomResponse
                                        {
                                            Id = r.Id,
                                            Name = r.Name,
                                            Description = r.Description
                                        }).FirstOrDefaultAsync();

                if (roomQuery != null)
                {
                    return Ok(roomQuery);
                }
                else
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Web method to retrieve all the active rooms
        /// </summary>
        /// <returns>List of rooms</returns>
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            try
            {

                //Getting active rooms list
                var roomsQuery = await (from r in dbContext.Room
                                        where r.StateID == (int)State.ACTIVE
                                        select new RoomResponse
                                        {
                                            Id = r.Id,
                                            Name = r.Name,
                                            Description = r.Description
                                        }).ToListAsync();

                if (roomsQuery != null)
                {
                    return Ok(roomsQuery);
                }
                else
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
