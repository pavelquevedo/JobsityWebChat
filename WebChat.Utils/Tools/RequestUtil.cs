using LumenWorks.Framework.IO.Csv;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebChat.Utils.Common.Models.Request;
using WebChat.Utils.Common.Models.Response;
using WebChat.Utils.Common.Constants;
using Path = WebChat.Utils.Common.Constants.Path;

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
        /// <param name="route">Api web method</param>
        /// <param name="method">Enum to indicate the Http verb of the request</param>
        /// <param name="accessToken">Access token for the authentication</param>
        /// <param name="objectRequest">object to attach in the request body</param>
        /// <returns>Object with ResponseType requested</returns>
        public static ApiResponse ExecuteWebMethod<ResponseType>(string route, Method method, string accessToken, object objectRequest = null, bool isExternalApi = false, string externalUrl = null)
        {
            RestClient restClient;

            //Check if is using internal api or an external one
            if (!isExternalApi)
                restClient = new RestClient(Path.API_URL);
            else
                restClient = new RestClient(externalUrl);

            //Declaring request object with its route
            RestRequest restRequest = new RestRequest(route, method);

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
                ApiResponse wSResponse = new ApiResponse() { StatusCode = response.Result.StatusCode };
                //Convert the result to the response type
                if (typeof(ResponseType) != Type.GetType("System.String"))
                {
                    if (response.Result.Content != string.Empty)
                    {
                        //Create a new instance of the ResponseType
                        var resultObject = Activator.CreateInstance<ResponseType>();
                        //Deserialize object and parse into Response Type object
                        resultObject = JsonConvert.DeserializeObject<ResponseType>(response.Result.Content);
                        //Return
                        wSResponse.Content = resultObject;
                        return wSResponse;                     
                    }                    
                }
                else
                {
                    //If requested content is a String, return content
                    wSResponse.Content = response.Result.Content;
                    return wSResponse;
                }
            }

            return null;

        }

    }
}
