using JobsityWebChat.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
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
            StockQuoteResponse processedQueue = null;

            //Build queue request
            StockQuoteRequest queueRequest = new StockQuoteRequest() {
                Command = "/stock=AAPL.US",
                RoomId = 1
            };

            //Generate user token
            var userToken = LoginHelper.GetUserToken();

            StockQuoteResponse quoteResponse = RequestUtil.GetStockQuoteResponse(queueRequest);

            // Act
            if (quoteResponse !=null)
            {
                processedQueue = RequestUtil.ExecuteWebMethod<StockQuoteResponse>(string.Format("api/message/sendBotMessage?roomId={0}", queueRequest.RoomId), Method.POST, userToken, quoteResponse);
            }

            // Checking if quoteResponse is not null
            Assert.IsNotNull(quoteResponse);
            //Check if processed queue is not null
            Assert.IsNotNull(processedQueue);
            //Check if message was sent
            Assert.AreEqual(processedQueue.State, WebChat.Utils.Common.Enum.State.SENT);
            //Checking content type
            Assert.IsInstanceOfType(processedQueue, typeof(StockQuoteResponse));
        }
    }
}
