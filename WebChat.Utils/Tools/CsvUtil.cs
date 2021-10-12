using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebChat.Utils.Common.Models.Response;

namespace WebChat.Utils.Tools
{
    /// <summary>
    /// Helper class to manage CSV files
    /// </summary>
    public class CsvUtil
    {
        /// <summary>
        /// Converts a formatted stock quote CSV file into StockQuote object
        /// </summary>
        /// <param name="csvString">Csv text</param>
        /// <returns>Populated StockQuote object</returns>
        public static StockQuoteResponse ConvertToStockQuoteResponse(string csvString)
        {
            //Generate stream from string
            Stream responseStream = GenerateStreamFromString(csvString);
            DataTable tableResult = new DataTable();

            using (var csvReader = new CsvReader(new StreamReader(responseStream), true))
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
                catch (Exception)
                {
                    throw new Exception("The stock command received doesn't exists.");                    
                }                

                //If list is not null, return the first object
                if (stockQuoteList != null)
                {
                    if (stockQuoteList.Count > 0)
                    {
                        //Since the csv just contains 1 row, we're returning only the first object
                        return stockQuoteList.FirstOrDefault();
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Method to convert a string into Stream
        /// </summary>
        /// <param name="csvString">String with CSV format</param>
        /// <returns>Stream generated from source string</returns>
        private static Stream GenerateStreamFromString(string csvString)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(csvString);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}