using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TradingViewBot.Data.Contexts;
using TradingViewBot.Models.Entities;

namespace TradingViewBot.BackgroundServices
{
    public class BotBackgroundService : BackgroundService
    {

        private readonly IServiceProvider _service;


        public BotBackgroundService(IServiceProvider service)
        {
            _service = service;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Set chrome options
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--incognito");
            options.AddArgument("--headless");

            //Create driver and set url
            IWebDriver driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://tr.tradingview.com/chart/?symbol=TVC%3ADXY");
            await Task.Delay(6000, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                IWebElement element = driver.FindElement(By.XPath(
                    @"/html/body/div[2]/div[6]/div/div[1]/div[1]/div[1]/div[2]/div[2]/div/div[2]/div[2]/span[1]/span[1]"));

                using (var scope = _service.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<BotContexts>();
                    var stock = new Stock()
                    {
                        StockCode = "DXY",
                        Price = double.Parse(element.Text)
                    };

                    dbContext.Stocks.Add(stock);
                    await dbContext.SaveChangesAsync(stoppingToken);
                }



                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
