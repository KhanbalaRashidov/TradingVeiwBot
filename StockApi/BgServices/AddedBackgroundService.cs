using System.Text;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using StockApi.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;
using StockApi.Data;
using StockApi.Services;

namespace StockApi.BgServices
{
    public class AddedBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ITelegramService _telegramService;

        public AddedBackgroundService(IServiceProvider serviceProvider, ITelegramService telegramService)
        {
            _serviceProvider = serviceProvider;
            _telegramService = telegramService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var options = new ChromeOptions();
            options.AddArgument("--incognito");
            var count = 0;

            StringBuilder stringBuilder = new StringBuilder();

            options.AddArgument("--headless");

            var driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://tr.tradingview.com/markets/stocks-turkey/market-movers-all-stocks/");
            Thread.Sleep(TimeSpan.FromSeconds(15));

            var loadButton = driver.FindElement(By.ClassName("loadButton-Hg5JK_G3"));
            loadButton.Click();
            Thread.Sleep(TimeSpan.FromSeconds(3));
            loadButton.Click();
            Thread.Sleep(TimeSpan.FromSeconds(3));
            loadButton.Click();
            Thread.Sleep(TimeSpan.FromSeconds(3));
            loadButton.Click();

            Thread.Sleep(TimeSpan.FromSeconds(3));

            var tBodyElements = driver.FindElements(By.XPath(
                @"/html/body/div[3]/div[4]/div/div/div/div[2]/div[2]/div/div[2]/div[2]/div/div/div/table/tbody"));
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                foreach (var tbodyElement in tBodyElements)
                {
                    var trElements = tbodyElement.FindElements(By.ClassName("row-EdyDtqqh"));

                    foreach (var trElement in trElements)
                    {
                        var stock = new Stock()
                        {
                            Code = trElement.FindElement(By.CssSelector("td.cell-TKkxf89L.left-TKkxf89L.cell-fixed-f5et_Mwd.onscroll-shadow > span > a")).Text,
                            CompanyName = trElement
                                .FindElement(By.CssSelector(
                                    "td.cell-TKkxf89L.left-TKkxf89L.cell-fixed-f5et_Mwd.onscroll-shadow > span > sup")).Text,
                            Price = trElement.FindElement(By.CssSelector("td:nth-child(2)")).Text,
                            ChangePrice = trElement.FindElement(By.CssSelector("td:nth-child(4) > span")).Text,
                            PercentageOfChange = trElement.FindElement(By.CssSelector("td:nth-child(3) > span")).Text
                        };

                        var context = scope.ServiceProvider.GetRequiredService<StocksContext>();

                        context.Stocks.Add(stock);
                        
                        await context.SaveChangesAsync(stoppingToken);
                        stringBuilder.AppendLine($"code:  {stock.Code}\t   price:  {stock.Price}\t date:  {DateTime.Now.ToString()}\t");
                        count++;

                        if (count == 20)
                        {
                            _telegramService.SendMessage(stringBuilder.ToString());
                            count = 0;
                            stringBuilder=new StringBuilder();
                            Thread.Sleep(TimeSpan.FromSeconds(2));
                        }
                      
                    }
                   
                }
                
            }
        }



        private double SplitData(string data)
        {
            return double.Parse(data.Substring(0, data.Length - 4));
        }
    }
}
