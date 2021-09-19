using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebChat.Utils.Common.Enum;
using WebChat.Utils.Common.Models.Request;
using WebChat.Utils.Common.Models.Response;

namespace JobsityWebChat.Controllers
{
    /// <summary>
    /// This controller manages users information and registry
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/user")]
    public class UserController : BaseController
    {
        /// <summary>
        /// Web method to retrieve all users data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getAll")]
        public async Task<IHttpActionResult> GetAll()
        {
            try
            {

                //Getting user list
                var usersQuery = await (from u in dbContext.User
                                        select new UserResponse
                                        {
                                            Id = u.Id,
                                            Login = u.Login,
                                            FirstName = u.FirstName,
                                            LastName = u.LastName
                                        }).ToListAsync();


                if (usersQuery != null)
                {
                    return Ok(usersQuery);
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Web method to register a new user
        /// </summary>
        /// <param name="model">User request model</param>
        /// <returns>User created object</returns>
        [HttpPost]
        [Route("register")]
        public async Task<IHttpActionResult> Register([FromBody] UserRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //Building new user model
                Models.User newUser = new Models.User()
                {
                    Login = model.Login,
                    Password = model.Password,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    CreationDate = DateTime.Now,
                    StateID = (int)State.ACTIVE
                };

                //Adding new user to the database
                dbContext.User.Add(newUser);
                int userID = await dbContext.SaveChangesAsync();

                //Building user response dto
                UserResponse userResponse = new UserResponse()
                {
                    Id = userID,
                    Login = newUser.Login,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName
                };

                //return new user
                return Ok(userResponse);

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
