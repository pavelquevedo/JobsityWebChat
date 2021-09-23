using JobsityWebChat.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using WebChat.Api.Controllers;
using WebChat.Utils.Common.Models.Request;
using WebChat.Utils.Common.Models.Response;
using WebChat.Utils.Tools;

namespace JobsityWebChat.Tests.Controllers
{
    [TestClass]
    public class UserControllerTest
    {
        [TestMethod]
        public async Task GetAll()
        {
            // Arrange
            UserController controller = new UserController();

            // Act
            var httpActionResult = await controller.GetAll();
            var contentResult = httpActionResult as OkNegotiatedContentResult<List<UserResponse>>;

            // Assert
            Assert.IsNotNull(contentResult);

            //Checking users data
            Assert.IsInstanceOfType(contentResult.Content, typeof(List<UserResponse>));

            //Check if statuscode = 200OK
            Assert.IsInstanceOfType(httpActionResult, typeof(OkNegotiatedContentResult<List<UserResponse>>));

        }

        [TestMethod]
        public async Task Register()
        {
            var firstName = RandomNameGenerator.Generate(5);
            var lastName = RandomNameGenerator.Generate(6);

            //Build user request
            UserRequest newUser = new UserRequest()
            {
                Login = firstName+lastName,
                FirstName = firstName,
                LastName = lastName,
                Password = Encrypt.GetSHA256("mypass")
            };

            // Arrange
            UserController controller = new UserController();

            // Act
            var httpActionResult = await controller.Register(newUser);
            
            var contentResult = httpActionResult as OkNegotiatedContentResult<UserResponse>;

            // Assert
            Assert.IsNotNull(contentResult);

            //Check if model contains new user ID
            Assert.AreNotEqual(0, contentResult.Content.Id);

            //Checking users data
            Assert.IsInstanceOfType(contentResult.Content, typeof(UserResponse));

            //Check if statuscode = 200OK
            Assert.IsInstanceOfType(httpActionResult, typeof(OkNegotiatedContentResult<UserResponse>));

        }


        [TestMethod]
        public async Task RegisterAnExistingUser()
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

            // Act
            var httpActionResult = await controller.Register(newUser);

            // Assert
            //Check if statuscode = 409 Conflict
            Assert.IsInstanceOfType(httpActionResult, typeof(ConflictResult));
            
        }

    }
}
