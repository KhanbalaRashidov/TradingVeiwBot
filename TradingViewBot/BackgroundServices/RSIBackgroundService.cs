using TradingViewBot.Data.Contexts;
using TradingViewBot.Models.Entities;
using TradingViewBot.Services;

namespace TradingViewBot.BackgroundServices
{
    public class RSIBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _service;
        private readonly ITelegramMessageService _telegramMessageService;

        public RSIBackgroundService(IServiceProvider service, ITelegramMessageService telegramMessageService)
        {
            _service = service;
         
            _telegramMessageService = telegramMessageService;
        }

        protected  override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _service.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<BotContexts>();

                    var prices = context.Set<Stock>().Where(x => x.StockCode == "DXY").Select(x => x.Price).ToArray();
                    var rsiPrices = CalculateRSI(prices, 14);
                    _telegramMessageService.SendMessage(rsiPrices.LastOrDefault().ToString());
                    Console.WriteLine(rsiPrices);
                    OfferBuyAndSell(prices);
                }

                await Task.Delay(20000,stoppingToken);
            }

        }

        private static double[] CalculateRSI(double[] prices, int period)
        {
            double[] rsiValues = new double[prices.Length];
            double avgGain = 0;
            double avgLoss = 0;

            // Calculate the average gain and loss for the first period
            for (int i = 1; i <= period; i++)
            {
                double priceDiff = prices[i] - prices[i - 1];
                if (priceDiff >= 0)
                {
                    avgGain += priceDiff;
                }
                else
                {
                    avgLoss += Math.Abs(priceDiff);
                }
            }

            avgGain /= period;
            avgLoss /= period;

            // Calculate the RSI for subsequent periods
            for (int i = period + 1; i < prices.Length; i++)
            {
                double priceDiff = prices[i] - prices[i - 1];
                double gain = 0;
                double loss = 0;

                if (priceDiff >= 0)
                {
                    gain = priceDiff;
                }
                else
                {
                    loss = Math.Abs(priceDiff);
                }

                avgGain = ((period - 1) * avgGain + gain) / period;
                avgLoss = ((period - 1) * avgLoss + loss) / period;

                double rs = avgGain / avgLoss;
                double rsi = 100 - (100 / (1 + rs));
                rsiValues[i] = rsi;
            }

            return rsiValues;
        }

        static double CalculateMovingAverage(double[] prices, int index, int period)
        {
            double sum = 0.0;
            for (int i = index - period + 1; i <= index; i++)
            {
                sum += prices[i];
            }
            return sum / period;
        }

        static void OfferBuyAndSell(double[] stockPrices)
        {
            int shortMovingAverage = 14;
            int longMovingAverage = 60;

            for (int i = longMovingAverage; i < 1; i++)
            {
                double shortMA = CalculateMovingAverage(stockPrices, i, shortMovingAverage);
                double longMA = CalculateMovingAverage(stockPrices, i, longMovingAverage);

                if (shortMA > longMA)
                {
                    Console.WriteLine("Buy recommendation. Date: {0}, Price: {1}", i, stockPrices[i]);
                }
                else if (shortMA < longMA)
                {
                    Console.WriteLine("Sell recommendation. Date: {0}, Price: {1}", i, stockPrices[i]);
                }
            }
        }


    }
}
