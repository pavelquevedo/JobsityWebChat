using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebChat.Utils.Common.Enum;

namespace WebChat.Utils.Common.Models.Response
{
    public class StockQuoteResponse
    {
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public Int64 Volume { get; set; }
        public State State { get; set; }

    }
}
