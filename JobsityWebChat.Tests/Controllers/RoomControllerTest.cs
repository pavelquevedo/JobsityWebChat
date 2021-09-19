using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using WebChat.Api.Controllers;
using WebChat.Utils.Common.Models.Response;

namespace JobsityWebChat.Tests.Controllers
{
    [TestClass]
    public class RoomControllerTest
    {
        [TestMethod]
        public async Task GetAll()
        {
            // Arrange
            RoomController controller = new RoomController();

            // Act
            var httpActionResult = await controller.GetAll();
            var contentResult = httpActionResult as OkNegotiatedContentResult<List<RoomResponse>>;

            // Assert
            Assert.IsNotNull(contentResult);

            //Checking users data
            Assert.IsInstanceOfType(contentResult.Content, typeof(List<RoomResponse>));

            //Check if statuscode = 200OK
            Assert.IsInstanceOfType(httpActionResult, typeof(OkNegotiatedContentResult<List<RoomResponse>>));

        }
    }
}
