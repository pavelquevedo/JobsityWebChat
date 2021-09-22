using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using WebChat.Api.Security;
using WebChat.Utils.Common.Enum;
using WebChat.Utils.Common.Models.Request;
using WebChat.Utils.Common.Models.Response;

namespace WebChat.Api.Controllers
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
                if (true)
                {
                    //Checking if user already exists
                    var userQuery = await (from u in dbContext.User
                                            where u.Login == model.Login
                                            select u).FirstOrDefaultAsync();

                    //If exists return conflict code
                    if (userQuery != null)
                    {
                        return Conflict();
                    }

                }

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
                await dbContext.SaveChangesAsync();

                //Getting token
                string token = TokenHandler.GenerateToken(newUser.Login);

                //Building user response dto
                UserResponse userResponse = new UserResponse()
                {
                    Id = newUser.Id,
                    Login = newUser.Login,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    AccessToken = token
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
