using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using WebChat.Api.Controllers;
using WebChat.Utils.Common.Models.Request;
using WebChat.Utils.Common.Models.Response;
using WebChat.Utils.Tools;

namespace JobsityWebChat.Tests.Controllers
{
    [TestClass]
    public sealed class MessageControllerTest
    {
        [TestMethod]
        public async Task GetMessages()
        {
            //arrange 
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

        [TestMethod]
        public async Task SendBotMessage()
        {
            //arrange
            int roomId = 1; //For room: Tech
            OkNegotiatedContentResult<StockQuoteResponse> contentResult = null;

            MessageController controller = new MessageController();

            StockQuoteRequest queueRequest = new StockQuoteRequest() {
                Command = "/stock=AAPL.US",
                RoomId = 1
            };

            StockQuoteResponse quoteResponse = RequestUtil.GetStockQuoteResponse(queueRequest);

            // Act
            if (quoteResponse !=null)
            {
                var httpActionResult = await controller.SendBotMessage(roomId, quoteResponse);
                contentResult = httpActionResult as OkNegotiatedContentResult<StockQuoteResponse>;
            }

            // Assert
            Assert.IsNotNull(quoteResponse);

            //Check if message was sent
            Assert.AreEqual(contentResult.Content.State, WebChat.Utils.Common.Enum.State.SENT);

            Assert.IsNotNull(contentResult);

            //Checking users data
            Assert.IsInstanceOfType(contentResult.Content, typeof(StockQuoteResponse));
        }
    }
}
