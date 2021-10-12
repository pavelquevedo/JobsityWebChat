using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebChat.Utils.Common.Constants
{
    /// <summary>
    /// Class to define the common paths used across the application
    /// </summary>
    public class Path
    {
        /// <summary>
        /// Returns main api url
        /// </summary>
        public static string API_URL
        {
            get
            {
                return ConfigurationManager.AppSettings["api_url"] == null
                    ? Environment.GetEnvironmentVariable("api_url")
                    : ConfigurationManager.AppSettings["api_url"];
            }
        }

        public static string STOOQAPI_URL
        {
            get
            {
                return ConfigurationManager.AppSettings["stooq_api"] == null 
                    ? Environment.GetEnvironmentVariable("stooq_api") 
                    : ConfigurationManager.AppSettings["stooq_api"];
            }
        }

        public class Url
        {
            /// <summary>
            /// SignalR Hub path
            /// </summary>
            public static string SignlRHub
            {
                get
                {
                    return API_URL + "signalr/hubs";
                }
            }
            /// <summary>
            /// SignalR path
            /// </summary>
            public static string SignlR
            {
                get
                {
                    return API_URL + "signalr/";
                }
            }

            public static string StockQuote
            {
                get
                {
                    return "?s={0}&f=sd2t2ohlcv&h&e=csv";
                }
            }
        }
    }
}
