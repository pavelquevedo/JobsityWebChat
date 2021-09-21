using WebChat.Utils.Common.Models.Request;

namespace WebChat.Utils.Interface
{
    public interface IQueueService
    {
        void CreateStockQueue(StockQuoteRequest queue);
    }
}
