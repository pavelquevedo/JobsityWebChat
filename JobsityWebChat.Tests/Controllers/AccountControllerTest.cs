using JobsityWebChat.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using WebChat.Utils.Common.Models.Request;
using WebChat.Utils.Common.Models.Response;

namespace JobsityWebChat.Tests.Controllers
{
    /// <summary>
    /// Summary description for AccountControllerTest
    /// </summary>
    [TestClass]
    public class AccountControllerTest
    {

        [TestMethod]
        public void Authenticate()
        {
            LoginRequest loginRequest = new LoginRequest()
            {
                Login = "pavel",
                Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4"
            };
            // Arrange
            AccountController controller = new AccountController();

            Task<IHttpActionResult> httpActionResult = controller.Authenticate(loginRequest);
            // Act
            var contentResult = httpActionResult.Result as OkNegotiatedContentResult<UserResponse>;
            
            // Assert
            Assert.IsNotNull(httpActionResult.Result);
            //Checking user data
            Assert.IsInstanceOfType(contentResult.Content, typeof(UserResponse));
            //Checking if returning token
            Assert.IsNotNull(contentResult.Content.AccessToken);

        }
    }
}
