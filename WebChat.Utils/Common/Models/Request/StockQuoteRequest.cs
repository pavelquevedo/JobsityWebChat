namespace WebChat.Utils.Common.Models.Request
{
    public class StockQuoteRequest
    {
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public string Command { get; set; }

    }
}
