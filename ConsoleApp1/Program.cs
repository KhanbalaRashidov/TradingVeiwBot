using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

Console.WriteLine("Hello, World!");
var stockCount = 0;
var option = new ChromeOptions();
option.AddArgument("--incognito");
option.AddArgument("--headless");

var driver = new ChromeDriver(option);
driver.Navigate().GoToUrl("https://tr.tradingview.com/markets/stocks-turkey/market-movers-all-stocks/");
Thread.Sleep(TimeSpan.FromSeconds(10));

var loadButton = driver.FindElement(By.ClassName("loadButton-Hg5JK_G3"));
loadButton.Click();
Thread.Sleep(TimeSpan.FromSeconds(3));
loadButton.Click();
Thread.Sleep(TimeSpan.FromSeconds(3));
loadButton.Click();
Thread.Sleep(TimeSpan.FromSeconds(3));
loadButton.Click();

Thread.Sleep(TimeSpan.FromSeconds(3));

var tbodyElements = driver.FindElements(
    By.XPath("/html/body/div[3]/div[4]/div/div/div/div[2]/div[2]/div/div[2]/div[2]/div/div/div/table/tbody"));
Console.WriteLine(DateTime.Now.ToLongTimeString());
foreach (var tbodyElement in tbodyElements)
{
    var trElements = tbodyElement.FindElements(By.ClassName("row-EdyDtqqh"));

    foreach (var trElement in trElements)
    {
        var stockCode = trElement.FindElement(By.CssSelector("td.cell-TKkxf89L.left-TKkxf89L.cell-fixed-f5et_Mwd.onscroll-shadow > span > a")).Text;
        var stockPrice = trElement.FindElement(By.CssSelector("td:nth-child(2)")).Text;
        var companyName = trElement
            .FindElement(By.CssSelector(
                "td.cell-TKkxf89L.left-TKkxf89L.cell-fixed-f5et_Mwd.onscroll-shadow > span > sup")).Text;

        var percentageOfPrice = trElement.FindElement(By.CssSelector("td:nth-child(3) > span")).Text;
        var changePrice = trElement.FindElement(By.CssSelector("td:nth-child(4) > span")).Text;

        Console.WriteLine($"Stock code:{stockCode} ---- price:{stockPrice} --- {companyName}  --- {percentageOfPrice}  ---  {changePrice}");

        stockCount++;
    }
}
Console.WriteLine(DateTime.Now.ToLongTimeString());

Console.WriteLine(stockCount);
Thread.Sleep(TimeSpan.FromSeconds(30));
Console.ReadKey();
driver.Quit();

Console.Write("Hello world!");

