using JobsityWebChat.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using WebChat.Utils.Common.Models.Request;
using WebChat.Utils.Common.Models.Response;

namespace JobsityWebChat.Tests.Controllers
{
    [TestClass]
    public class UserControllerTest
    {
        [TestMethod]
        public void GetAll()
        {
            // Arrange
            UserController controller = new UserController();

            Task<IHttpActionResult> httpActionResult = controller.GetAll();
            // Act
            var contentResult = httpActionResult.Result as OkNegotiatedContentResult<List<UserResponse>>;

            // Assert
            Assert.IsNotNull(httpActionResult.Result);

            //Checking users data
            Assert.IsInstanceOfType(contentResult.Content, typeof(List<UserResponse>));

            //Check if statuscode = 200OK
            Assert.IsInstanceOfType(httpActionResult.Result, typeof(OkNegotiatedContentResult<List<UserResponse>>));

        }

        [TestMethod]
        public void Register()
        {
            //Build user request
            UserRequest newUser = new UserRequest()
            {
                Login = "pavelq",
                FirstName = "Pavel",
                LastName = "Quevedo",
                Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4"
            };

            // Arrange
            UserController controller = new UserController();

            Task<IHttpActionResult> httpActionResult = controller.Register(newUser);
            // Act
            var contentResult = httpActionResult.Result as OkNegotiatedContentResult<UserResponse>;

            // Assert
            Assert.IsNotNull(httpActionResult.Result);

            //Check if model contains new user ID
            Assert.AreNotEqual(0, contentResult.Content.Id);

            //Checking users data
            Assert.IsInstanceOfType(contentResult.Content, typeof(UserResponse));

            //Check if statuscode = 200OK
            Assert.IsInstanceOfType(httpActionResult.Result, typeof(OkNegotiatedContentResult<UserResponse>));

        }
    }
}
