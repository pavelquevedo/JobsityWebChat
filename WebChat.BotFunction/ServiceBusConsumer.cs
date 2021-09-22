using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using WebChat.Utils.Common.Models.Request;
using WebChat.Utils.Common.Models.Response;
using WebChat.Utils.Tools;

namespace WebChat.QueueConsumerFunction
{
    public static class ServiceBusConsumer
    {
        [FunctionName("ServiceBusConsumer")]
        public static void Run([ServiceBusTrigger("stockqueue", Connection = "queueConnectionString")] string myQueueItem, ILogger log)
        {
            //Parsing request
            StockQuoteRequest stockQueueRequest = JsonConvert.DeserializeObject<StockQuoteRequest>(myQueueItem);

            //Getting response from api
            StockQuoteResponse stockQuoteResponse = RequestUtil.GetStockQuoteResponse(stockQueueRequest);

            //Executing web stooq.com api method
            StockQuoteResponse result = RequestUtil.ExecuteWebMethod<StockQuoteResponse>(string.Format("api/message/sendBotMessage?roomId={0}", stockQueueRequest.RoomId), Method.POST, string.Empty, stockQuoteResponse);

            log.LogInformation("Queue item processed: " + JsonConvert.SerializeObject(result));
        }
    }
}
