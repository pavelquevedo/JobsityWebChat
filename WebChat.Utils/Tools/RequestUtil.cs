using Newtonsoft.Json;
using RestSharp;
using System;
using System.Configuration;
using System.Net;

namespace WebChat.Utils.Tools
{
    public class RequestUtil
    {
        public static dynamic ExecuteWebMethod<ResponseType>(string url, Method method, string accessToken, object objectRequest)
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

            //Execute petition
            IRestResponse response = restClient.Execute(restRequest);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //Create a new instance of the ResponseType
                var resultObject = Activator.CreateInstance<ResponseType>();
                //Convert the result to the response type
                resultObject = JsonConvert.DeserializeObject<ResponseType>(response.Content);

                return resultObject;
            }
            else
            {
                return null;
            }
        }
    }
}
