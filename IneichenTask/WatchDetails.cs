using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;

namespace IneichenTask
{
    class WatchDetails
    {
        HtmlWeb web;
        HtmlDocument doc;
        private static string connStr = "Server=DESKTOP-B32RQ3U;Database=Ineichen;Integrated Security=True;";
        public WatchDetails(WatchesModel model)
        {
            web = new HtmlWeb();
            doc = web.Load(model.URL);
            SetWatchDetails(model);
            model.PrintDetails();
            WatchesModel oldDataModel = GetWatchByIDAndAuctionID(model.AuctionID, model.ID);
            if (oldDataModel == null)
            {
                AddWatch(model);
                return;
            }
            else if (!model.Equals(oldDataModel))
            {
                UpdateWatch(model);
            }

        }

        #region Add Watch
        public void AddWatch(WatchesModel watch)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand("PR_Watches_AddWatch", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@AuctionID", (object)watch.AuctionID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@URL", (object)watch.URL ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Condition", (object)watch.Condition ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Created", (object)watch.Created ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Currency", (object)watch.Currency ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Description", (object)watch.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DimensionsRaw", (object)watch.DimensionsRaw ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EstimatePriceRaw", (object)watch.EstimatePriceRaw ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ID", (object)watch.ID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsUnsold", (object)watch.IsUnsold ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsSold", (object)watch.IsSold ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PhotosRaw", (object)watch.PhotosRaw ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PriceRaw", (object)watch.PriceRaw ?? DBNull.Value);
                    command.Parameters.AddWithValue("@StartingBid", (object)watch.StartingBid ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Title", (object)watch.Title ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Box", (object)watch.Box ?? DBNull.Value);
                    command.Parameters.AddWithValue("@BraceletOrStrapMaterial", (object)watch.BraceletOrStrapMaterial ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CaseMaterial", (object)watch.CaseMaterial ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CaseNo", (object)watch.CaseNo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DialColor", (object)watch.DialColor ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DialStamp", (object)watch.DialStamp ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Location", (object)watch.Location ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Manufacturer", (object)watch.Manufacturer ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Model", (object)watch.Model ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Movement", (object)watch.Movement ?? DBNull.Value);
                    command.Parameters.AddWithValue("@MovementNo", (object)watch.MovementNo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Papers", (object)watch.Papers ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ReferenceNo", (object)watch.ReferenceNo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Width", (object)watch.Width ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Height", (object)watch.Height ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Depth", (object)watch.Depth ?? DBNull.Value);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
        #endregion

        #region Update Watch By Id and AuctionID
        public void UpdateWatch(WatchesModel watch)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand("PR_Watches_EditWatchByIDAndAuctionID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@AuctionID", (object)watch.AuctionID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@URL", (object)watch.URL ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Condition", (object)watch.Condition ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Created", (object)watch.Created ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Currency", (object)watch.Currency ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Description", (object)watch.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DimensionsRaw", (object)watch.DimensionsRaw ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EstimatePriceRaw", (object)watch.EstimatePriceRaw ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ID", (object)watch.ID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsUnsold", (object)watch.IsUnsold ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsSold", (object)watch.IsSold ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PhotosRaw", (object)watch.PhotosRaw ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PriceRaw", (object)watch.PriceRaw ?? DBNull.Value);
                    command.Parameters.AddWithValue("@StartingBid", (object)watch.StartingBid ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Title", (object)watch.Title ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Box", (object)watch.Box ?? DBNull.Value);
                    command.Parameters.AddWithValue("@BraceletOrStrapMaterial", (object)watch.BraceletOrStrapMaterial ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CaseMaterial", (object)watch.CaseMaterial ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CaseNo", (object)watch.CaseNo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DialColor", (object)watch.DialColor ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DialStamp", (object)watch.DialStamp ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Location", (object)watch.Location ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Manufacturer", (object)watch.Manufacturer ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Model", (object)watch.Model ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Movement", (object)watch.Movement ?? DBNull.Value);
                    command.Parameters.AddWithValue("@MovementNo", (object)watch.MovementNo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Papers", (object)watch.Papers ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ReferenceNo", (object)watch.ReferenceNo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Width", (object)watch.Width ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Height", (object)watch.Height ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Depth", (object)watch.Depth ?? DBNull.Value);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region Get Watch By ID And Auction ID
        public WatchesModel GetWatchByIDAndAuctionID(string auctionID, int id)
        {
            WatchesModel watch = null;

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand("PR_Watches_SelectWatchByIDAndAuctionID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AuctionID", (object)auctionID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ID", (object)id ?? DBNull.Value);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            watch = new WatchesModel
                            {
                                AuctionID = reader["AuctionID"] != DBNull.Value ? reader["AuctionID"].ToString() : null,
                                URL = reader["URL"] != DBNull.Value ? reader["URL"].ToString() : null,
                                Condition = reader["Condition"] != DBNull.Value ? reader["Condition"].ToString() : null,
                                Created = reader["Created"] != DBNull.Value ? reader["Created"].ToString() : null,
                                Currency = reader["Currency"] != DBNull.Value ? reader["Currency"].ToString() : null,
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                                DimensionsRaw = reader["DimensionsRaw"] != DBNull.Value ? reader["DimensionsRaw"].ToString() : null,
                                EstimatePriceRaw = reader["EstimatePriceRaw"] != DBNull.Value ? reader["EstimatePriceRaw"].ToString() : null,
                                ID = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : default,
                                IsUnsold = reader["IsUnsold"] != DBNull.Value ? Convert.ToBoolean(reader["IsUnsold"]) : (bool?)null,
                                IsSold = reader["IsSold"] != DBNull.Value ? Convert.ToBoolean(reader["IsSold"]) : (bool?)null,
                                PhotosRaw = reader["PhotosRaw"] != DBNull.Value ? reader["PhotosRaw"].ToString() : null,
                                PriceRaw = reader["PriceRaw"] != DBNull.Value ? reader["PriceRaw"].ToString() : null,
                                StartingBid = reader["StartingBid"] != DBNull.Value ? reader["StartingBid"].ToString() : null,
                                Title = reader["Title"] != DBNull.Value ? reader["Title"].ToString() : null,
                                Box = reader["Box"] != DBNull.Value ? reader["Box"].ToString() : null,
                                BraceletOrStrapMaterial = reader["BraceletOrStrapMaterial"] != DBNull.Value ? reader["BraceletOrStrapMaterial"].ToString() : null,
                                CaseMaterial = reader["CaseMaterial"] != DBNull.Value ? reader["CaseMaterial"].ToString() : null,
                                CaseNo = reader["CaseNo"] != DBNull.Value ? reader["CaseNo"].ToString() : null,
                                DialColor = reader["DialColor"] != DBNull.Value ? reader["DialColor"].ToString() : null,
                                DialStamp = reader["DialStamp"] != DBNull.Value ? reader["DialStamp"].ToString() : null,
                                Location = reader["Location"] != DBNull.Value ? reader["Location"].ToString() : null,
                                Manufacturer = reader["Manufacturer"] != DBNull.Value ? reader["Manufacturer"].ToString() : null,
                                Model = reader["Model"] != DBNull.Value ? reader["Model"].ToString() : null,
                                Movement = reader["Movement"] != DBNull.Value ? reader["Movement"].ToString() : null,
                                MovementNo = reader["MovementNo"] != DBNull.Value ? reader["MovementNo"].ToString() : null,
                                Papers = reader["Papers"] != DBNull.Value ? reader["Papers"].ToString() : null,
                                ReferenceNo = reader["ReferenceNo"] != DBNull.Value ? reader["ReferenceNo"].ToString() : null,
                                Width = reader["Width"] != DBNull.Value ? reader["Width"].ToString() : null,
                                Height = reader["Height"] != DBNull.Value ? reader["Height"].ToString() : null,
                                Depth = reader["Depth"] != DBNull.Value ? reader["Depth"].ToString() : null
                            };
                        }
                    }
                }
            }

            return watch;
        }
        #endregion

        #region Set WatchDetails
        private void SetWatchDetails(WatchesModel model)
        {
            SetID(model);
            SetCondition(model);
            SetCreated(model);
            SetCurrency(model);
            SetDescription(model);
            SetEstimatedPriceRaw(model);
            SetDimensions(model);
            SetPhotosRaw(model);
            SetPriceRaw(model);
            SetStartingBid(model);
            SetTitle(model);
            SetBox(model);
            SetBraceletOrStrapMaterial(model);
            SetCaseMaterial(model);
            SetCaseNo(model);
            SetDialColor(model);
            SetDialStamp(model);
            SetLocation(model);
            SetManufacturer(model);
            SetModel(model);
            SetMovement(model);
            SetMovementNo(model);
            SetPapers(model);
            SetReferenceNo(model);
        }
        #endregion

        #region Set Title
        private void SetTitle(WatchesModel model)
        {
            string titleXPath = "//h1[contains(@class,'product__brand')] | //h2[contains(@class,'product__model')]";
            model.Title = GetDataByXPath("Title", titleXPath);
        }
        #endregion

        #region Set Condition
        private void SetCondition(WatchesModel model)
        {
            string conditionXPath = "//h3[contains(text(),'Condition')]/following-sibling::table//tr";
            model.Condition = GetDataByXPath("Condition", conditionXPath);
        }
        #endregion

        #region Set Created
        private void SetCreated(WatchesModel model)
        {
            string createdXPath = "//td[contains(.,'Year')]/following-sibling::td";
            model.Created = GetDataByXPath("Created", createdXPath);
        }
        #endregion

        #region Set Currency
        private void SetCurrency(WatchesModel model)
        {
            model.Currency = "CHF";
        }
        #endregion

        #region Set Description
        private void SetDescription(WatchesModel model)
        {
            string descriptionXPath = "//table[(following-sibling::h3[contains(text(),'Condition')])]//td";
            model.Description = GetDataByXPath("Description", descriptionXPath);
        }
        #endregion

        #region Set Dimension
        private void SetDimensions(WatchesModel model)
        {
            string dimensionsXPath = "//td[contains(text(),'D=')]/following-sibling::td";
            model.DimensionsRaw = GetDataByXPath("Dimensions", dimensionsXPath);
            List<string> patterns = new List<string>
            {
                //with depth
                @"(?<width>\d+([\.,]\d+)?\s?\w*)\s?[xхX]\s?(?<height>\d+([\.,]\d+)?\s?\w*)\s?(?:[xхX]\s?(?<depth>\d+([\.,]\d+)?)\s?\w*)\s?",
                //without depth
                @"(?<width>\d+([\.,]\d+)?\s?\w*)\s?[xхX]\s?(?<height>\d+([\.,]\d+)?\s?\w*)\s?",
                @"(?<width>\d+([\.,]\d+)?\s?\w*)\s?\/\s?(?<height>\d+([\.,]\d+)?\s?\w*)",
                @"(?<width>\d+([\.,]\d+)?\s?\w*)"
            };
            foreach (string pattern in patterns)
            {
                if (model.ID == 35)
                {
                    Console.WriteLine("");
                }
                Regex regex = new Regex(pattern);
                if (model.DimensionsRaw != null)
                {
                    Match match = regex.Match(model.DimensionsRaw);
                    if (match.Success)
                    {
                        model.Width = match.Groups["width"].Value;
                        model.Height = match.Groups["height"].Success ? match.Groups["height"].Value : null;
                        model.Depth = match.Groups["depth"].Success ? match.Groups["depth"].Value : null;
                        break;
                    }
                }
            }
        }
        #endregion

        #region Set Estimated Raw
        private void SetEstimatedPriceRaw(WatchesModel model)
        {
            string estimatePriceRawXPath = "//div[contains(@class,'estimator-review__value')]";
            model.EstimatePriceRaw = GetDataByXPath("Estimate", estimatePriceRawXPath);
        }
        #endregion

        #region Set ID
        private void SetID(WatchesModel model)
        {
            string IDXPath = "//div[contains(@class,'product__num')]/span";
            model.ID = Convert.ToInt32(GetDataByXPath("ID", IDXPath));
        }
        #endregion

        #region Set Photos
        private void SetPhotosRaw(WatchesModel model)
        {
            string photosXPath = "//div[contains(@class,'product__gallery')]//img";
            model.PhotosRaw = GetAttributeDataByXPath("Photos", "src", photosXPath);
        }
        #endregion

        #region Set Price Raw
        private void SetPriceRaw(WatchesModel model)
        {
            string priceXPath = "//span[contains(text(),'Hammer price')]/following-sibling::strong";
            model.PriceRaw = GetDataByXPath("Price", priceXPath);
            if (model.PriceRaw == null)
            {
                model.IsSold = false;
                model.IsUnsold = false;
                return;
            }
            if (model.PriceRaw.Contains("CHF"))
            {
                model.IsSold = true;
                model.IsUnsold = false;
                return;
            }
            if (model.PriceRaw.Contains("Pass"))
            {
                model.IsUnsold = true;
                model.IsSold = false;
                return;
            }
            model.IsSold = false;
            model.IsUnsold = false;
        }
        #endregion

        #region Set Starting Bid
        private void SetStartingBid(WatchesModel model)
        {
            string startingBidXPath = "//span[contains(text(),'Starting bid')]/following-sibling::strong";
            model.StartingBid = GetDataByXPath("Starting Bid", startingBidXPath);
        }
        #endregion

        #region Set Box
        private void SetBox(WatchesModel model)
        {
            string boxXPath = "//td[contains(.,'Box')]/following-sibling::td";
            model.Box = GetDataByXPath("Box", boxXPath);
        }
        #endregion

        #region Set Bracelet Or Strap Material
        private void SetBraceletOrStrapMaterial(WatchesModel model)
        {
            string braceletOrStrapMaterialXPath = "//table[(following-sibling::h3[contains(text(),'Condition')])]//td[contains(text(),'Strap')]/following-sibling::td";
            model.BraceletOrStrapMaterial = GetDataByXPath("bracelet or Strap material", braceletOrStrapMaterialXPath);
        }
        #endregion

        #region Set Case Material
        private void SetCaseMaterial(WatchesModel model)
        {
            string caseMaterialXPath = "//td[contains(.,'Case')]/following-sibling::td";
            model.CaseMaterial = GetDataByXPath("Case Material", caseMaterialXPath);
        }
        #endregion

        #region Set Case No
        private void SetCaseNo(WatchesModel model)
        {
            string caseNoXPath = "//td[contains(text(),'Case No.')]/following-sibling::td";
            model.CaseNo = GetDataByXPath("Case No", caseNoXPath);
        }
        #endregion

        #region Set Dial Color
        private void SetDialColor(WatchesModel model)
        {
            string dialColorXPath = "//tr[contains(.,'Dial')]";
            model.DialColor = GetDataByXPath("Dial Color", dialColorXPath);
        }
        #endregion

        #region Set Dial Stamp
        private void SetDialStamp(WatchesModel model)
        {
            string dialStampXPath = "//tr[contains(.,'Dial')]";
            model.DialStamp = GetDataByXPath("Dial stamp", dialStampXPath);
        }
        #endregion

        #region Set Location
        private void SetLocation(WatchesModel model)
        {
            string locationXPath = "//span[contains(text(),'Location')]/following-sibling::strong";
            model.Location = GetDataByXPath("Location", locationXPath);
        }
        #endregion

        #region Set Manufacturer
        private void SetManufacturer(WatchesModel model)
        {
            string manufacturerXPath = "//h1[contains(@class,'product__brand')]";
            model.Manufacturer = GetDataByXPath("Manufacturer", manufacturerXPath);
        }
        #endregion

        #region Set Model
        private void SetModel(WatchesModel model)
        {
            string modelXPath = "//h2[contains(@class,'product__model')]";
            model.Model = GetDataByXPath("Model", modelXPath);
        }
        #endregion

        #region Set Movement
        private void SetMovement(WatchesModel model)
        {
            string movementXPath = "//td[contains(.,'Movement')]/following-sibling::td";
            model.Movement = GetDataByXPath("Movement", movementXPath);
        }
        #endregion

        #region Set Movement No
        private void SetMovementNo(WatchesModel model)
        {
            string movementNoXPath = "//td[contains(.,'Movement No.')]/following-sibling::td";
            model.MovementNo = GetDataByXPath("Movement No", movementNoXPath);
        }
        #endregion

        #region Set Papers
        private void SetPapers(WatchesModel model)
        {
            string papersXPath = "//td[contains(text(),'Papers')]/following-sibling::td";
            model.Papers = GetDataByXPath("Papers", papersXPath);
        }
        #endregion

        #region Set Reference No
        private void SetReferenceNo(WatchesModel model)
        {
            string referenceNoXPath = "//td[contains(.,'Ref.')]/following-sibling::td";
            model.ReferenceNo = GetDataByXPath("Reference No", referenceNoXPath);
        }
        #endregion

        #region Get Data by XPath
        private string GetDataByXPath(string name, string xPath)
        {
            try
            {
                HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(xPath);

                if (nodes == null || nodes.Count == 0)
                {
                    Console.WriteLine(name + " node not found");
                    return null;
                }

                StringBuilder tempString = new StringBuilder();

                foreach (HtmlNode node in nodes)
                {
                    if (node != null)
                    {
                        tempString.Append(node.InnerText.Trim() + " ");
                    }
                }
                return tempString.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while setting the " + name + " : " + ex.Message);
                return null;
            }
        }
        #endregion

        #region Get Attribute Data By XPath
        private string GetAttributeDataByXPath(string name, string attributeName, string xPath)
        {
            try
            {
                HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(xPath);

                if (nodes == null || nodes.Count == 0)
                {
                    Console.WriteLine(name + " node not found");
                    return null;
                }

                StringBuilder tempString = new StringBuilder();

                foreach (HtmlNode node in nodes)
                {
                    if (node != null)
                    {
                        tempString.Append("https://ineichen.com" + node.GetAttributeValue(attributeName, "").Trim() + " ");
                    }
                }
                return tempString.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while setting the " + name + " : " + ex.Message);
                return null;
            }
        }
        #endregion

    }
}
