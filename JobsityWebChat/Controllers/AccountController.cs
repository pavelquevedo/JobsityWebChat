using WebChat.Api.Security;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using WebChat.Utils.Common.Models.Request;
using WebChat.Utils.Common.Models.Response;

namespace WebChat.Api.Controllers
{
    /// <summary>
    /// This controller manages the api authentication
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/accounts")]
    public class AccountController : BaseController
    {
        /// <summary>
        /// Authentication method validates if user exists and if the credentials are correct,
        /// and generates auth token.
        /// </summary>
        /// <param name="loginRequest">Login request with login and password</param>
        /// <returns>User response with auth token and user data</returns>
        [HttpPost]
        [Route("authenticate")]
        public async Task<IHttpActionResult> Authenticate([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //Getting logged user from db
                var userQuery = await (from u in dbContext.User
                                  where u.Login == loginRequest.Login && u.Password == loginRequest.Password
                                  select u).ToListAsync();

                var loggedUser = userQuery.FirstOrDefault();

                //If user exists return an user response with 200 status code
                if (loggedUser != null)
                {
                    //Getting token
                    string token = TokenHandler.GenerateToken(loggedUser.Login);

                    //Building user response dto
                    UserResponse userResponse = new UserResponse()
                    {
                        Id = loggedUser.Id,
                        AccessToken = token,
                        Login = loggedUser.Login,
                        FirstName = loggedUser.FirstName,
                        LastName = loggedUser.LastName
                    };

                    return Ok(userResponse);
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
    }
}
