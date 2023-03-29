namespace StockApi.Services
{
    public interface ITelegramService
    {
        void SendMessage(string message);
    }
}
