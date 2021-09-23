using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using WebChat.Api.Controllers;
using WebChat.Utils.Common.Models.Request;
using WebChat.Utils.Common.Models.Response;

namespace JobsityWebChat.Tests.Controllers
{
    /// <summary>
    /// Test class with unit tests for AccountController
    /// </summary>
    [TestClass]
    public class AccountControllerTest
    {

        [TestMethod]
        public async Task Authenticate()
        {
            LoginRequest loginRequest = new LoginRequest()
            {
                Login = "pavel",
                Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4"
            };
            // Arrange
            AccountController controller = new AccountController();
            
            // Act
            var httpActionResult = await controller.Authenticate(loginRequest);
            var contentResult = httpActionResult as OkNegotiatedContentResult<UserResponse>;
            
            // Assert
            Assert.IsNotNull(contentResult);
            //Checking data type
            Assert.IsInstanceOfType(contentResult.Content, typeof(UserResponse));
            //Checking if returning token
            Assert.IsNotNull(contentResult.Content.AccessToken);
        }

        [TestMethod]
        public async Task AuthenticationFailed()
        {
            LoginRequest loginRequest = new LoginRequest()
            {
                Login = "pavel",
                Password = "incorrect password"
            };
            // Arrange
            AccountController controller = new AccountController();

            // Act
            var httpActionResult = await controller.Authenticate(loginRequest);
            var contentResult = httpActionResult as UnauthorizedResult;

            // Assert
            Assert.IsNotNull(contentResult);
            //Checking data type
            Assert.IsInstanceOfType(httpActionResult, typeof(UnauthorizedResult));
        }
    }
}
