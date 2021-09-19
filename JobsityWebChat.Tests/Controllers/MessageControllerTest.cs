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
    public sealed class MessageControllerTest
    {
        [TestMethod]
        public async Task GetMessages()
        {
            //arrange - left blank on purpose
            int roomId = 1; //For room: Tech
            int userId = 1; //For User: Pavel Quevedo

            MessageController controller = new MessageController();

            // Act
            var httpActionResult = await controller.GetRoomMessages(roomId, userId);
            var contentResult = httpActionResult as OkNegotiatedContentResult<List<MessageResponse>>;

            // Assert
            Assert.IsNotNull(contentResult);

            //Checking users data
            Assert.IsInstanceOfType(contentResult.Content, typeof(List<MessageResponse>));

            //Check if statuscode = 200OK
            Assert.IsInstanceOfType(httpActionResult, typeof(OkNegotiatedContentResult<List<MessageResponse>>));

        }
    }
}
