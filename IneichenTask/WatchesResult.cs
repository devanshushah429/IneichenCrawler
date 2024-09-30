using System;
using System.Threading;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace IneichenTask
{

    class WatchesResult
    {
        private IWebDriver driver;
        private HtmlDocument doc;
        private string AuctionID;

        public WatchesResult(string watchesPageUrl, string auctionID)
        {
            //ChromeOptions options = new ChromeOptions();
            //options.AddArgument("headless"); // Optional: Run in headless mode if you don't need UI
            driver = new ChromeDriver();

            try
            {
                driver.Navigate().GoToUrl(watchesPageUrl);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                
                ScrollToLoadMoreContent(wait);

                string pageSource = driver.PageSource;

                doc = new HtmlDocument();
                doc.LoadHtml(pageSource);

                this.AuctionID = auctionID;
                SetWatchesData();
            }
            finally
            {
                driver.Quit();
            }
        }

        private void ScrollToLoadMoreContent(WebDriverWait wait)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            bool moreContentLoaded = true;

            while (moreContentLoaded)
            {
                long initialScrollHeight = (long)js.ExecuteScript("return document.body.scrollHeight");

                js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                Thread.Sleep(2000);

                long newScrollHeight = (long)js.ExecuteScript("return document.body.scrollHeight");

                if (newScrollHeight == initialScrollHeight)
                {
                    moreContentLoaded = false; 
                }
            }
        }

        public void SetWatchesData()
        {
            string URLXPath = "//a[contains(@class,'lot-item__info')]";
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(URLXPath);

            if (nodes != null)
            {
                foreach (HtmlNode node in nodes)
                {
                    WatchesModel watchModel = new WatchesModel();
                    watchModel.URL = "https://ineichen.com" + node.GetAttributeValue("href", "");
                    watchModel.AuctionID = this.AuctionID;
                    WatchDetails watchDetails = new WatchDetails(watchModel);
                    Console.WriteLine("URL: " + watchModel.URL);
                }
            }
            else
            {
                Console.WriteLine("No nodes found with the given XPath.");
            }
        }
    }
}
