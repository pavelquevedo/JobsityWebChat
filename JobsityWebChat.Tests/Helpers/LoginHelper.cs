using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using WebChat.Api.Controllers;
using WebChat.Utils.Common.Models.Request;
using WebChat.Utils.Common.Models.Response;

namespace JobsityWebChat.Tests.Helpers
{
    public class LoginHelper
    {


        public static string GetUserToken()
        {
            LoginRequest loginRequest = new LoginRequest()
            {
                Login = "pavel",
                Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4"
            };

            AccountController accountController = new AccountController();
            Task<IHttpActionResult> authTask = Task.Run(() => accountController.Authenticate(loginRequest));
            authTask.Wait();

            var contentResult = authTask.Result as OkNegotiatedContentResult<UserResponse>;
            if (authTask.Result != null)
            {
                return contentResult.Content.AccessToken;
            }
            return string.Empty;
        }
    }
}
