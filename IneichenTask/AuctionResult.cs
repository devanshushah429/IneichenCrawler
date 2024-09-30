using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace IneichenTask
{

    class AuctionResult
    {
        private static string connStr = "Server=DESKTOP-B32RQ3U;Database=Ineichen;Integrated Security=True;";
        public static string url = "https://ineichen.com/auctions/past/";
        HtmlWeb web = new HtmlWeb();
        HtmlDocument doc;
        private static string multipleSpaceRegex = @"\s+";

        public AuctionResult()
        {
            doc = web.Load(url);
            GetAllAuctionsData();
        }
        private void GetAllAuctionsData()
        {
            string baseXPath = "//div[contains(@class,'auctions-list')]//div[@id]";
            HtmlNodeCollection auctionsNodes = doc.DocumentNode.SelectNodes(baseXPath);
            foreach (HtmlNode node in auctionsNodes)
            {
                AuctionModel model = new AuctionModel();
                SetModelData(node, model);
                AuctionModel oldDataModel = GetAuctionDataByAuctionID(model.AuctionID);
                if(oldDataModel == null)
                {
                    InsertAuctionIntoDatabase(model);
                }else
                {
                    if(!model.Equals(oldDataModel))
                    {
                        UpdateAuctionIntoDatabase(model);
                    }
                }
                WatchesResult result = new WatchesResult(model.WatchesPageURL, model.AuctionID);
            }
        }

        #region Set Model Data
        private static void SetModelData(HtmlNode node, AuctionModel model)
        {
            SetTitle(node, model);
            SetImageURL(node, model);
            SetWatchesPageURL(node, model);
            SetDateRaw(node, model);
            SetLocation(node, model);
            model.PrintData();
        }
        #endregion

        #region Set Title
        private static void SetTitle(HtmlNode node, AuctionModel model)
        {
            try
            {
                if (node == null)
                {
                    Console.WriteLine("Error: node is null.");
                    return;
                }

                string titleXPath = ".//h2[contains(@class,'auction-item__name')]";
                HtmlNode titleNode = node.SelectSingleNode(titleXPath);

                if (titleNode == null)
                {
                    Console.WriteLine("Error: title node not found.");
                    return;
                }

                model.Title = titleNode.InnerText.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }
        #endregion

        #region SetImageURL
        private static void SetImageURL(HtmlNode node, AuctionModel model)
        {
            try
            {
                if (node == null)
                {
                    Console.WriteLine("Error: node is null.");
                    return;
                }

                string imageUrlXPath = ".//a[contains(@class,'auction-item__image')]/img";
                HtmlNode imageURLNode = node.SelectSingleNode(imageUrlXPath);

                if (imageURLNode == null)
                {
                    Console.WriteLine("Error: Image url node not found.");
                    return;
                }
                string imageUrlsrc = imageURLNode.GetAttributeValue("src", string.Empty);
                Uri absoluteUrl = new Uri(new Uri(url), imageUrlsrc);
                model.ImageURL = absoluteUrl.ToString().Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }
        #endregion

        #region Watches Page URL
        private static void SetWatchesPageURL(HtmlNode node, AuctionModel model)
        {
            try
            {
                if (node == null)
                {
                    Console.WriteLine("Error: node is null.");
                    return;
                }

                string watchesPageURLXPath = ".//div[contains(@class,'auction-item__btns')]/a";
                HtmlNode watchesPageUrlNode = node.SelectSingleNode(watchesPageURLXPath);

                if (watchesPageUrlNode == null)
                {
                    Console.WriteLine("Error: Watches Page url node not found.");
                    return;
                }
                string linkText = watchesPageUrlNode.GetAttributeValue("href", string.Empty);
                Uri absoluteUrlLink = new Uri(new Uri(url), linkText);
                model.WatchesPageURL = absoluteUrlLink.ToString().Trim();
                String pattern = @"auctions\/(.*)\/";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(model.WatchesPageURL);
                if (match.Success)
                {
                    model.AuctionID = match.Groups[1].Value;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }
        #endregion

        #region Set Date 
        private static void SetDateRaw(HtmlNode node, AuctionModel model)
        {
            try
            {
                if (node == null)
                {
                    Console.WriteLine("Error: date node is null.");
                    return;
                }

                string dateXPath = ".//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]";
                HtmlNode dateNode = node.SelectSingleNode(dateXPath);

                if (dateNode == null)
                {
                    Console.WriteLine("Error: date node not found.");
                    return;
                }
                string dateString = Regex.Replace(dateNode.InnerText.Trim(), multipleSpaceRegex, " ");
                List<string> datePatterns = new List<string>()
                {
                    @"^(\d+)\s?-\s?(\d+)\s(\w+)\s(\d{2}:\d{2}\sCET)$",
                @"^(\d+)\s-\s(\d+)\s(\w+)\s(\d{4})$",
                @"^(\d+)\s-\s(\d+)\s(\w+)$",
                @"^(\d)+\s(\w+)\s-\s(\d+)\s(\w+)$",
                @"^(\d+)\s(\w+),\s(\d{2}:\d{2}\sCET)\s(\d+)\s(\w+),\s(\d{2}:\d{2}\sCET)$",
                @"^(\d+)\s(\w+),\s(\d{2}:\d{2}\s\(CET\))$",
                @"^(\d+)\s(\w+)\s-\s(\d+)\s(\w+)\s(\d{2}:\d{2}\sCET)$",
                @"^(\d+)\s(\w+)\s-\s(\d+)\s(\w+)\s(\d{4})$",
                @"^(\d+)\s(\w+)\s(\d{4}),\s(\d{2}:\d{2}\s\(CET\))$",
                @"^(\d+)\s(\w+)\s(\d{4})\s-\s(\d+)\s(\w+)\s(\d{4})$"
                };
                model.DateTime = dateString;
                foreach (string pattern in datePatterns)
                {
                    Regex dateRegex = new Regex(pattern);
                    dateString = Regex.Replace(dateString.Trim(), multipleSpaceRegex, " ");
                    Match match = dateRegex.Match(dateString);
                    if (match.Success)
                    {
                        model.DateTime = dateString;
                        break;
                    }
                    else
                    {
                        model.DateTime = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }
        #endregion

        #region Watches Page URL
        private static void SetLocation(HtmlNode node, AuctionModel model)
        {
            try
            {
                if (node == null)
                {
                    Console.WriteLine("Error: node is null.");
                    return;
                }

                string locationXPath = ".//div[@class='auction-date-location']//div[i[contains(@class,'mdi-map-marker-outline')]|i[contains(@class,'mdi-web')]]";
                HtmlNode locationNode = node.SelectSingleNode(locationXPath);

                if (locationNode == null)
                {
                    Console.WriteLine("Error: Location node not found.");
                    return;
                }
                model.Location = locationNode.InnerText.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }
        #endregion

        #region Insert Auction Into Database
        public static void InsertAuctionIntoDatabase(AuctionModel model)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PR_Auctions_AddAuction",conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Title", model.Title);
                cmd.Parameters.AddWithValue("@ImageURL", model.ImageURL);
                cmd.Parameters.AddWithValue("@AuctionID", model.AuctionID);
                cmd.Parameters.AddWithValue("@DateTime", model.DateTime);
                cmd.Parameters.AddWithValue("@Location", model.Location);
                cmd.Parameters.AddWithValue("@WatchesPageURL", model.WatchesPageURL);
                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine("Rows affected: " + rowsAffected);
                conn.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        #endregion

        #region Update Auction Into Database
        public static void UpdateAuctionIntoDatabase(AuctionModel model)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PR_Auctions_UpdateByAuctionID",conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Title", model.Title);
                cmd.Parameters.AddWithValue("@ImageURL", model.ImageURL);
                cmd.Parameters.AddWithValue("@AuctionID", model.AuctionID);
                cmd.Parameters.AddWithValue("@DateTime", model.DateTime);
                cmd.Parameters.AddWithValue("@Location", model.Location);
                cmd.Parameters.AddWithValue("@WatchesPageURL", model.WatchesPageURL);
                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine("Rows affected: " + rowsAffected);
                conn.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        #endregion

        #region Get Auction Data From Database
        public static AuctionModel GetAuctionDataByAuctionID(string auctionID)
        {
            AuctionModel model = null;
            try
            {
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PR_Auctions_GetDataByAuctionID",conn);
                cmd.Parameters.AddWithValue("@AuctionID", auctionID);
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        model = new AuctionModel()
                        {
                            ImageURL = reader["ImageURL"].ToString(),
                            AuctionID = reader["AuctionID"].ToString(),
                            Title = reader["Title"].ToString(),
                            DateTime = reader["DateTime"].ToString(),
                            Location = reader["Location"].ToString(),
                            WatchesPageURL = reader["WatchesPageURL"].ToString()
                        };
                    }
                }
                conn.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return model;
        }
        #endregion

    }
}
