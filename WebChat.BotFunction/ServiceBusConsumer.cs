using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
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
        [FixedDelayRetry(5, "00:00:05")]
        public static void Run([ServiceBusTrigger("stockqueue", Connection = "queueConnectionString")] string myQueueItem, ILogger log)
        {
            //Parsing request
            StockQuoteRequest stockQueueRequest = JsonConvert.DeserializeObject<StockQuoteRequest>(myQueueItem);

            try
            {
                //Executing web stooq.com api method
                StockQuoteResponse stockQuoteResponse = RequestUtil.GetStockQuoteResponse(stockQueueRequest);

                if (stockQuoteResponse != null)
                {
                    //Getting response from api    
                    StockQuoteResponse result = RequestUtil.ExecuteWebMethod<StockQuoteResponse>(string.Format("api/message/sendBotMessage?roomId={0}", stockQueueRequest.RoomId), Method.POST, string.Empty, stockQuoteResponse);
                    //Printing in log item processed
                    log.LogInformation("Queue item processed: " + JsonConvert.SerializeObject(result));
                }
                else
                {
                    //If an invalid command was sent or the request was unsuccessful, it will return null and "invalida command" will be printed in the log
                    log.LogInformation("Invalid command for request: " + myQueueItem);
                }
            }
            catch (System.Exception e)
            {
                //Here i would add a logic to send an email to notify that the bot is failing
                log.LogError("An exception occurred: " + e.Message);
                //throw ex;
            }
        }
    }
}
