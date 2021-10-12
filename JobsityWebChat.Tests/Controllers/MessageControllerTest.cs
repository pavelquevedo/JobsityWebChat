using JobsityWebChat.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Results;
using WebChat.Api.Controllers;
using WebChat.Utils.Common.Constants;
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
            string stockCommand = "AAPL.US";
            StockQuoteResponse processedQueue = null;

            //Build queue request
            string stooqRoute = string.Format(Path.Url.StockQuote, stockCommand);

            //Generate user token
            var userToken = LoginHelper.GetUserToken();

            ApiResponse stockQueryReponse = RequestUtil
                   .ExecuteWebMethod<String>(stooqRoute, Method.GET, string.Empty, null, true, Path.STOOQAPI_URL);

            if (stockQueryReponse.StatusCode == HttpStatusCode.OK)
            {
                //Convert string result into CSV
                StockQuoteResponse quoteResponse = CsvUtil.ConvertToStockQuoteResponse(stockQueryReponse.Content.ToString());

                // Act
                if (quoteResponse != null)
                {
                    processedQueue = (StockQuoteResponse)RequestUtil
                        .ExecuteWebMethod<StockQuoteResponse>(string.Format("api/messages/{0}", roomId), Method.POST, userToken, quoteResponse).Content;
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
            else
            {
                Assert.Fail("Stooq api is not returning valid status code.");
            }           
        }
    }
}
