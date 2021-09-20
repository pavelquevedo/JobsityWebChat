using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Configuration;
using System.Text;
using WebChat.Utils.Common.Models.Request;

namespace WebChat.Utils.Services
{
    public class QueueProducerService
    {
        public void AddMessageToQueue(QueueRequest queueRequest)
        {
            //Getting parameters from web.config
            string hostName = ConfigurationManager.AppSettings["RabbitMQ_Host"];
            string queueName = ConfigurationManager.AppSettings["RabbitMQ_QueueName"];

            var factory = new ConnectionFactory() { HostName = hostName };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //Declaring queue
                    channel.QueueDeclare(queueName, false, false, false, null);

                    //Serializing and encrypting object
                    string serializedRequest = JsonConvert.SerializeObject(queueRequest);
                    
                    //Encoding message
                    var body = Encoding.UTF8.GetBytes(serializedRequest);

                    //Send queue 
                    channel.BasicPublish(string.Empty, queueName, null, body);
                }
            }
        }
    }
}
