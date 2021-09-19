using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using WebChat.Utils.Common.Enum;
using WebChat.Utils.Common.Models.Response;

namespace WebChat.Api.Controllers
{
    /// <summary>
    /// This controller manages rooms information
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/room")]
    public class RoomController : BaseController
    {
        /// <summary>
        /// Web method to retrieve all the active rooms
        /// </summary>
        /// <returns>List of rooms</returns>
        [HttpGet]
        [Route("getAll")]
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
