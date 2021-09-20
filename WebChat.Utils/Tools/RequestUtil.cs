using Newtonsoft.Json;
using RestSharp;
using System;
using System.Configuration;

namespace WebChat.Utils.Tools
{
    public class RequestUtil
    {
        public static dynamic ExecuteWebMethod<ResponseType>(string url, Method method, string accessToken, object objectRequest)
        {
            //Create a new instance of the ResponseType
            var resultObject = Activator.CreateInstance<ResponseType>();

            RestClient restClient = new RestClient(ConfigurationManager.AppSettings["api_url"]);
            RestRequest restRequest = new RestRequest(url, method);

            //Add request body
            if (objectRequest != null)
            {
                restRequest.AddJsonBody(objectRequest);
            }

            //Execute petition
            IRestResponse response = restClient.Execute(restRequest);

            //Convert the result to the response type
            resultObject = JsonConvert.DeserializeObject<ResponseType>(response.Content);

            return resultObject;

        }
    }
}
