using Microsoft.Azure.ServiceBus;
using System.Configuration;
using System.Text;
using System.Text.Json;
using WebChat.Utils.Common.Models.Request;
using WebChat.Utils.Interface;

namespace WebChat.Utils.Services
{
    public class StockQueueProducer : IQueueService
    {
        /// <summary>
        /// Method which inserts a new queue request into the azure service bus
        /// </summary>
        /// <param name="queue">Queue request object</param>
        public void CreateStockQueue(StockQuoteRequest queue)
        {
            //Getting connection string and queue name from web.config
            var connectionString = ConfigurationManager.AppSettings["ServiceBusConnectionString"];
            var queueName = ConfigurationManager.AppSettings["QueueName"];

            //instanciate queue service bus client
            var queueClient = new QueueClient(connectionString, queueName);

            //Serializing request
            var serializedQueue = JsonSerializer.Serialize(queue);

            //Sending queue
            var sendTask = queueClient.SendAsync(new Message(Encoding.UTF8.GetBytes(serializedQueue)));
            sendTask.Wait();
        }
    }
}
