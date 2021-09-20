using Newtonsoft.Json;
using RestSharp;
using System;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;

namespace WebChat.Utils.Tools
{
    /// <summary>
    /// This class manages any api request
    /// </summary>
    public class RequestUtil
    {
        /// <summary>
        /// Executes an specific web method
        /// </summary>
        /// <typeparam name="ResponseType">Desired data type of the resultset</typeparam>
        /// <param name="url">Api web method</param>
        /// <param name="method">Enum to indicate the Http verb of the request</param>
        /// <param name="accessToken">Access token for the authentication</param>
        /// <param name="objectRequest">object to attach in the request body</param>
        /// <returns></returns>
        public static dynamic ExecuteWebMethod<ResponseType>(string url, Method method, string accessToken, object objectRequest = null)
        {
            RestClient restClient = new RestClient(ConfigurationManager.AppSettings["api_url"]);
            RestRequest restRequest = new RestRequest(url, method);

            //Add request body
            if (objectRequest != null)
            {
                restRequest.AddJsonBody(objectRequest);
            }

            //Add authentication header if web method doesn't allow anonymous requests
            if (accessToken != string.Empty)
            {
                restRequest.AddHeader("Content-Type", "application/json");
                restRequest.AddHeader("Authorization", "Bearer " + accessToken);
            }

            //Execute petition and wait to continue
            var response = Task.Run(() => restClient.ExecuteAsync(restRequest));
            response.Wait();

            if (response.Result.StatusCode == HttpStatusCode.OK)
            {
                //Create a new instance of the ResponseType
                var resultObject = Activator.CreateInstance<ResponseType>();
                //Convert the result to the response type
                resultObject = JsonConvert.DeserializeObject<ResponseType>(response.Result.Content);

                return resultObject;
            }
            else
            {
                return null;
            }
        }
    }
}
