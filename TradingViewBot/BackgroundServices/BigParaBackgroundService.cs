using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
namespace TradingViewBot.BackgroundServices
{
    public class BigParaBackgroundService : BackgroundService
    {
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--incognito");
            //options.AddArgument("--headless");

            var driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://bigpara.hurriyet.com.tr/borsa/canli-borsa//");
            await Task.Delay(25000, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
               
                var elements =
                    driver.FindElement(
                        By.XPath(@"/html/body/div[3]/div[4]/div/div[1]/div/div[3]/div[5]/div/div/div[2]"));

                var ulElements = elements.FindElements(By.ClassName("live-stock-item"));

                Console.WriteLine();
                foreach (var ulElement in ulElements)
                {
                    var ilElements = ulElement.FindElements(By.ClassName("cell048"));
                    var stockName = ulElement.FindElement(By.CssSelector(" li.cell064.tal.arrow > a:nth-child(2)"));
                    Console.WriteLine(stockName.Text);
                    foreach (var ilElement in ilElements)
                    {
                        Console.WriteLine(ilElement.Text);
                    }
                }

                //await Task.Delay(10000, stoppingToken);
                driver.Navigate().Refresh();
                await Task.Delay(2500, stoppingToken);
            }

            driver.Quit();
        }
    }
}
