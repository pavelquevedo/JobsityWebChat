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
            var azureFunctionUrl = Environment.GetEnvironmentVariable("api_url");
            var webConfigUrl = ConfigurationManager.AppSettings["api_url"];

            //If is running library from WebChat.Api it will use webConfigUrl, if is from Azure Function will use azureFunctionUrl
            RestClient restClient = new RestClient(webConfigUrl != null ? webConfigUrl : azureFunctionUrl);
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

        /// <summary>
        /// Method which invokes stook.com api and performs requested csv 
        /// convertion
        /// </summary>
        /// <param name="queueRequest"></param>
        /// <returns>StockQuoteResponse object with request's data</returns>
        public static StockQuoteResponse GetStockQuoteResponse(StockQuoteRequest queueRequest)
        {
            //Splitting stock command
            string[] messageSplit = queueRequest.Command.Split('=');

            StockQuoteResponse stockQuoteResponse = null;

            if (messageSplit.Length == 2)
            {
                if (messageSplit[1].Length > 0)
                {
                    //var url = "https://stooq.com/q/l/"; //ConfigurationManager.AppSettings["stooq_api"];
                    var url = Environment.GetEnvironmentVariable("stooq_api");
                    var tableResult = new DataTable();

                    //Creating simple web request
                    WebRequest webRequest = WebRequest.Create(url + string.Format("?s={0}&f=sd2t2ohlcv&h&e=csv", messageSplit[1]));

                    //Web request properties
                    webRequest.Method = Method.GET.ToString();
                    webRequest.PreAuthenticate = true;
                    webRequest.ContentType = "text/csv; charset=utf-8";
                    webRequest.Timeout = 10000;

                    var httpResponse = (HttpWebResponse)webRequest.GetResponse();
                    //If successful request
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        using (var csvReader = new CsvReader(new StreamReader(httpResponse.GetResponseStream()), true))
                        {
                            //Converting the stream response into a datatable
                            tableResult.Load(csvReader);
                            var stockQuoteList = new List<StockQuoteResponse>();
                            try
                            {
                                //Converting table result to StockQuoteResonpse objects
                                stockQuoteList = (from row in tableResult.AsEnumerable()
                                                  select new StockQuoteResponse()
                                                  {
                                                      Symbol = row["Symbol"].ToString(),
                                                      Date = DateTime.Parse(row["Date"].ToString()),
                                                      Time = row["Time"].ToString(),
                                                      Open = Decimal.Parse(row["Close"].ToString()),
                                                      High = Decimal.Parse(row["High"].ToString()),
                                                      Low = Decimal.Parse(row["Low"].ToString()),
                                                      Close = Decimal.Parse(row["Close"].ToString()),
                                                      Volume = Int64.Parse(row["Volume"].ToString()),
                                                      State = Common.Enum.State.CREATED
                                                  }).ToList();
                            }
                            catch (Exception ex)
                            {
                                //If Symbol is not recognized by the api, return null object
                                return stockQuoteResponse;
                            }

                            if (stockQuoteList != null)
                            {
                                if (stockQuoteList.Count > 0)
                                {
                                    //Since the csv just contains 1 row, we're returning only the first object
                                    stockQuoteResponse = stockQuoteList.FirstOrDefault();
                                    return stockQuoteResponse;
                                }
                            }
                        }
                    }
                    else
                    {

                    }
                }
            }

            return stockQuoteResponse;
        }

    }
}
