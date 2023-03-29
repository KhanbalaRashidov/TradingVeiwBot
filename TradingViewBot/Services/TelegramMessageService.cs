using Telegram.Bot;

namespace TradingViewBot.Services
{
    public class TelegramMessageService : ITelegramMessageService
    {
        private string chatId = "-918334854";
        private string token = "6054499458:AAHgvlmL6TfLEJ_Ln2yCsMKVOQIpXfsFa9M";

        public async void SendMessage(string message)
        {
            var botClient = new TelegramBotClient(token);
            await botClient.SendTextMessageAsync(chatId, message);
        }
    }
}
