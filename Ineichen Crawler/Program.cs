using HtmlAgilityPack;
using System.Text.RegularExpressions;
using Ineichen_Crawler.View_Models;
using System.Data.SqlClient;
using System;
using System.Diagnostics;

namespace Ineichen_Crawler
{
    public class Program
    {

        private static readonly string url = "https://ineichen.com/auctions/past/";

        static void Main(string[] args)
        {
            ScrapeAndStoreData().Wait();
        }

        private static async Task ScrapeAndStoreData()
        {
            Stopwatch totalStopwatch = Stopwatch.StartNew();
        
            var connectionString = "Server=DESKTOP-AH3AP4P\\MSSQLSERVER01;Database=Ineichen;Trusted_Connection=True;TrustServerCertificate=True";
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(url);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(response);

            var auctionNodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='auctions-list']//div[@class='auction-item']");
            if (auctionNodes == null)
            {
                Console.WriteLine("No auction nodes found.");
                return;
            }

            foreach (var node in auctionNodes)
            {
                Console.WriteLine(totalStopwatch.ElapsedMilliseconds);
                try
                {
                    var titleNode = node.SelectSingleNode(".//h2[@class='auction-item__name']/a");
                    var descriptionNode = node.SelectSingleNode(".//div[contains(@class, 'auction-date-location')]");
                    var imageUrlNode = node.SelectSingleNode(".//a[@class='auction-item__image']/img");
                    var linkNode = node.SelectSingleNode(".//a[@class='auction-item__image']");
                    var lotCountNode = node.SelectSingleNode(".//div[@class='auction-item__btns']/a");
                    //var dateNode = node.SelectSingleNode(".//div[contains(@class, 'auction-date-location')]/b");
                    var dateNode = node.SelectSingleNode(".//div[contains(@class, 'auction-date-location')]/div[1]");
                    var timeNode = node.SelectSingleNode(".//div[contains(@class, 'auction-date-location')]/span");
                    var locationNode = node.SelectSingleNode(".//div[contains(@class, 'auction-date-location')]/div[2]/span");

                    var dateNewGenerated = ExtractDate(dateNode?.InnerText?.Trim());

                    var auction = new VMAuction
                    {
                        Title = titleNode?.InnerText.Trim(),
                        Description = descriptionNode?.InnerText.Trim(),
                        ImageUrl = "https://ineichen.com" + imageUrlNode?.GetAttributeValue("src", string.Empty),
                        Link = "https://ineichen.com" + linkNode?.GetAttributeValue("href", string.Empty),
                        LotCount = lotCountNode != null ? ExtractLotCount(lotCountNode.InnerText.Trim()) : 0,
                        StartDate = dateNode != null ? dateNewGenerated.StartDate : null,
                        StartMonth = dateNode != null ? dateNewGenerated.StartMonth : null,
                        StartYear = dateNode != null ? dateNewGenerated.StartYear : null,
                        StartTime = dateNewGenerated.StartTime,
                        EndDate = dateNode != null ? dateNewGenerated.EndDate : null,
                        EndMonth = dateNode != null ? dateNewGenerated.EndMonth : null,
                        EndYear = dateNode != null ? dateNewGenerated.EndYear : null,
                        EndTime = dateNewGenerated.EndTime,
                        Location = locationNode?.SelectSingleNode(".//a")?.InnerText.Trim() ?? locationNode?.InnerText.Trim()
                    };

                    // Print or store the auction object
                    Console.WriteLine($"Title: {auction.Title}");
                    Console.WriteLine($"ImageUrl: {auction.ImageUrl}");
                    Console.WriteLine($"Link: {auction.Link}");
                    Console.WriteLine($"LotCount: {auction.LotCount}");
                    Console.WriteLine($"StartDate: {auction.StartDate}");
                    Console.WriteLine($"EndDate: {auction.EndDate}");
                    Console.WriteLine($"Location: {auction.Location}");
                    Console.WriteLine();

                    // Save to the database
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        var sql = @"INSERT INTO Auctions (Title, Description, ImageUrl, Link, LotCount, Location, StartDate, EndDate, StartMonth, EndMonth, StartYear, EndYear, StartTime, EndTime)
            VALUES (@Title, @Description, @ImageUrl, @Link, @LotCount, @Location, @StartDate, @EndDate, @StartMonth, @EndMonth, @StartYear, @EndYear, @StartTime, @EndTime)";

                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Title", auction.Title);
                            command.Parameters.AddWithValue("@Description", auction.Description);
                            command.Parameters.AddWithValue("@ImageUrl", auction.ImageUrl);
                            command.Parameters.AddWithValue("@Link", auction.Link);
                            command.Parameters.AddWithValue("@LotCount", auction.LotCount);
                            command.Parameters.AddWithValue("@Location", auction.Location);
                            command.Parameters.AddWithValue("@StartDate", auction.StartDate);
                            command.Parameters.AddWithValue("@EndDate", auction.EndDate);
                            command.Parameters.AddWithValue("@StartMonth", auction.StartMonth);
                            command.Parameters.AddWithValue("@EndMonth", auction.EndMonth);
                            command.Parameters.AddWithValue("@StartYear", auction.StartYear);
                            command.Parameters.AddWithValue("@EndYear", auction.EndYear);
                            command.Parameters.AddWithValue("@StartTime", auction.StartTime);
                            command.Parameters.AddWithValue("@EndTime", auction.EndTime);
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("--------------------------------------------------------");
                }
            }
        }


        private static int ExtractLotCount(string text)
        {
            // Example text: "View 90 lots"
            int LotCount = 0;
            if (!string.IsNullOrEmpty(text))
            {
                string pattern = @"View\s*(?<lotNumber>\d{1,})\s*lots";
                Match match = Regex.Match(text, pattern);

                if (match.Success && int.TryParse(match.Groups["lotNumber"].Value, out int lotNumber))
                {
                    LotCount = lotNumber;
                }
            }
            return LotCount;
        }

        private static VMDate ExtractDate(string dateText)
        {
            VMDate generatedDate = new VMDate();

             string[] Patterns = new[]
                {

                 @"(?<StartDate>\d{1,2})\s*(?<StartMonth>(?:JANUARY|FEBRUARY|MARCH|APRIL|MAY|JUNE|JULY|AUGUST|SEPTEMBER|OCTOMBER|NOVEMBER|DECEMBER|JAN|FRB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC))\s*(?<StartYear>\d{4})?\s*-\s*(?<EndDate>\d{1,2})\s*(?<EndMonth>(?:JANUARY|FEBRUARY|MARCH|APRIL|MAY|JUNE|JULY|AUGUST|SEPTEMBER|OCTOMBER|NOVEMBER|DECEMBER|JAN|FRB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC))\s*(?<StartYear>\d{4})",
                 @"(?<StartDate>\d{1,2})\s*(?<StartMonth>(?:JANUARY|FEBRUARY|MARCH|APRIL|MAY|JUNE|JULY|AUGUST|SPETEMBER|OCTOMBER|NOVEMBER|DECEMBER|JAN|FRB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC))\s*\,?\s*(?<StartTime>\d{1,2}:\d{1,2}\s*\(?(CET)?\)?)?\s*(?<EndDate>\d{1,2})\s*(?<EndMonth>(?:JANUARY|FEBRUARY|MARCH|APRIL|MAY|JUNE|JULY|AUGUST|SPETEMBER|OCTOMBER|NOVEMBER|DECEMBER|JAN|FRB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC))\s*\,?\s*(?<EndTime>\d{1,2}:\d{1,2}\s*\(?(CET)?\)?)?",
                 @"(?<StartDate>\d{1,2})\s*(?<StartMonth>(?:JANUARY|FEBRUARY|MARCH|APRIL|MAY|JUNE|JULY|AUGUST|SPETEMBER|OCTOMBER|NOVEMBER|DECEMBER|JAN|FRB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC))\s*-\s*(?<EndDate>\d{1,2})\s*(?<EndMonth>(?:JANUARY|FEBRUARY|MARCH|APRIL|MAY|JUNE|JULY|AUGUST|SPETEMBER|OCTOMBER|NOVEMBER|DECEMBER|JAN|FRB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC))\s*(?<StartTime>\d{1,2}:\d{1,2}\s*\(?(CET)?\)?)?",
                 @"(?<StartDate>\d{1,2})\s*-?\s*(?<EndDate>\d{1,2})\s*(?<StartMonth>(?:JANUARY|FEBRUARY|MARCH|APRIL|MAY|JUNE|JULY|AUGUST|SPETEMBER|OCTOMBER|NOVEMBER|DECEMBER|JAN|FRB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC))\s*(?<StartYear>\d{4})",
                 @"(?<StartDate>\d{1,2})\s*-\s*(?<EndDate>\d{1,2})\s*(?<StartMonth>(?:JANUARY|FEBRUARY|MARCH|APRIL|MAY|JUNE|JULY|AUGUST|SPETEMBER|OCTOMBER|NOVEMBER|DECEMBER|JAN|FRB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC))\s*(?<StartTime>\d{1,2}:\d{1,2}\s*\(?CET\)?)?",
                 @"(?<StartDate>\d{1,2})\s*(?<StartMonth>(?:JANUARY|FEBRUARY|MARCH|APRIL|MAY|JUNE|JULY|AUGUST|SPETEMBER|OCTOMBER|NOVEMBER|DECEMBER|JAN|FRB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC))(,)?\s*(?<StartTime>\d{1,2}:\d{1,2}\s*\(?(CET)?\)?)?",
                };
            foreach (var pattern in Patterns)
            {
                var match = Regex.Match(dateText, pattern, RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    generatedDate.StartDate = int.Parse(match.Groups["StartDate"].Value);
                    generatedDate.StartMonth = match.Groups["StartMonth"].Value ;
                    generatedDate.StartYear = match.Groups["StartYear"].Success ? int.Parse(match.Groups["StartYear"].Value) : 2024;
                    generatedDate.EndMonth = match.Groups["EndMonth"].Success ? match.Groups["EndMonth"].Value : generatedDate.StartMonth;
                    generatedDate.EndDate = match.Groups["EndDate"].Success ? int.Parse(match.Groups["EndDate"].Value) : generatedDate.StartDate;
                    generatedDate.EndYear = match.Groups["EndYear"].Success ? int.Parse(match.Groups["EndYear"].Value) : generatedDate.StartYear;
                    generatedDate.StartTime = match.Groups["StartTime"].Success? match.Groups["StartTime"].Value: "";
                    generatedDate.EndTime = match.Groups["EndTime"].Success ? match.Groups["EndTime"].Value : generatedDate.StartTime;


                    break;
                }
                else
                {
                    generatedDate.StartDate = 0;
                    generatedDate.StartMonth = "";
                    generatedDate.StartYear = 0;
                    generatedDate.EndMonth = "";
                    generatedDate.EndDate = 0;
                    generatedDate.EndYear = 0;
                    generatedDate.StartTime = "";
                    generatedDate.EndTime = "";
                }
            }
            return generatedDate;
        }
    }
}
