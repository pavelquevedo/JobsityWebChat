using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net;
using WebChat.Utils.Common.Constants;
using WebChat.Utils.Common.Enum;
using WebChat.Utils.Common.Models.Request;
using WebChat.Utils.Common.Models.Response;
using WebChat.Utils.Tools;

namespace WebChat.QueueConsumerFunction
{
    /// <summary>
    /// Azure function which acts like a bot reading directly from stockqueue everytime a queue is added
    /// </summary>
    public static class ServiceBusConsumer
    {
        [FunctionName("ServiceBusConsumer")]
        //[FixedDelayRetry(5, "00:00:05")]
        public static void Run([ServiceBusTrigger("stockqueue", Connection = "queueConnectionString")] string myQueueItem, ILogger log)
        {
            //Parsing request
            StockQuoteRequest stockQueueRequest = JsonConvert.DeserializeObject<StockQuoteRequest>(myQueueItem);
            //Set up user credentials
            LoginRequest loginRequest = new LoginRequest()
            {
                Login = Environment.GetEnvironmentVariable("auth_user"),
                Password = Environment.GetEnvironmentVariable("user_password")
            };
            try
            {
                string stockCommand = stockQueueRequest.Command;
                //Getting command sub index 1, since possible command syntax errors were managed in the client and Hub
                string stooqRoute = string.Format(Path.Url.StockQuote, stockCommand.Split('=')[1]);
                //Executing web stooq.com api method
                string stockQueryReponse = RequestUtil
                    .ExecuteWebMethod<String>(stooqRoute, Method.GET, string.Empty, null, true, Path.STOOQAPI_URL).Content.ToString();
                //Parse stock response csv into StockQueueRequest object
                StockQuoteResponse stockQuoteResponse = CsvUtil.ConvertToStockQuoteResponse(stockQueryReponse);

                if (stockQuoteResponse != null)
                {
                    //Getting usertoken
                    ApiResponse loggedUserResponse = RequestUtil
                        .ExecuteWebMethod<UserResponse>("api/accounts/authenticate", Method.POST, string.Empty, loginRequest);

                    //Check if login succeded
                    if (loggedUserResponse.StatusCode == HttpStatusCode.OK)
                    {
                        UserResponse loggedUser = (UserResponse)loggedUserResponse.Content;
                        //Send bot's message using message post api endpoint  
                        StockQuoteResponse result = (StockQuoteResponse)RequestUtil
                            .ExecuteWebMethod<StockQuoteResponse>(string.Format("api/messages/{0}", stockQueueRequest.RoomId), Method.POST, loggedUser.AccessToken, stockQuoteResponse).Content;
                        //Printing in log item processed if the STATE is CREATED
                        if (result.State == State.CREATED)
                        {
                            log.LogInformation("Queue item processed: " + JsonConvert.SerializeObject(result));
                        }
                        else
                        {
                            //Printing error if command was invalid
                            log.LogInformation("Invalid command for request: " + myQueueItem);
                        }
                    }
                    else
                    {
                        log.LogError("User provided for sending messages has invalid credentials.");
                    }                                     
                }
                else
                {
                    //If an invalid command was sent or the request was unsuccessful,
                    //it will return null and "invalida command" will be printed in the log
                    log.LogInformation("Invalid command for request: " + myQueueItem);
                }
            }
            catch (Exception e)
            {
                //Here i would add a logic to send an email to notify that the bot is failing
                log.LogError("An exception occurred: " + e.Message);
            }
        }
    }
}
