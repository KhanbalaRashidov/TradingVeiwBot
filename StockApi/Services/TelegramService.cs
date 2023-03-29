using Telegram.Bot;

namespace StockApi.Services
{
    public class TelegramService:ITelegramService
    {
        private string chatId = "-918334854";
        private string token = "6054499458:AAHgvlmL6TfLEJ_Ln2yCsMKVOQIpXfsFa9M";
        public async void SendMessage(string message)
        {
            var client = new TelegramBotClient(token);
            await client.SendTextMessageAsync(chatId, message);
        }
    }
}
