using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IneichenTask
{
    class WatchesModel
    {
        public string AuctionID { get; set; }
        public string URL { get; set; }
        public string Condition { get; set; }
        public string Created { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public string DimensionsRaw { get; set; }
        public string EstimatePriceRaw { get; set; }
        public int ID { get; set; }
        public bool? IsUnsold { get; set; }
        public bool? IsSold { get; set; }
        public string PhotosRaw { get; set; }
        public string PriceRaw { get; set; }
        public string StartingBid { get; set; }
        public string Title { get; set; }
        public string Box { get; set; }
        public string BraceletOrStrapMaterial { get; set; }
        public string CaseMaterial { get; set; }
        public string CaseNo { get; set; }
        public string DialColor { get; set; }
        public string DialStamp { get; set; }
        public string Location { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Movement { get; set; }
        public string MovementNo { get; set; }
        public string Papers { get; set; }
        public string ReferenceNo { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Depth { get; set; }
        public void PrintDetails()
        {
            Console.WriteLine("------------------------------------------------------------------------------------");
            Console.WriteLine("AuctionID: " + this.AuctionID);
            Console.WriteLine("URL: " + this.URL);
            Console.WriteLine("Condition: " + this.Condition);
            Console.WriteLine("Created: " + this.Created);
            Console.WriteLine("Currency: " + this.Currency);
            Console.WriteLine("Description: " + this.Description);
            Console.WriteLine("Dimensions: " + this.DimensionsRaw);
            Console.WriteLine("Estimated Price: " + this.EstimatePriceRaw);
            Console.WriteLine("ID: " + this.ID);
            Console.WriteLine("IsSold: "+ this.IsSold);
            Console.WriteLine("IsUnsold: "+ this.IsUnsold);
            Console.WriteLine("Photos: " + this.PhotosRaw);
            Console.WriteLine("Price Raw: " + this.PriceRaw);
            Console.WriteLine("Starting Bid: " + this.StartingBid);
            Console.WriteLine("Title: " + this.Title);
            Console.WriteLine("Box: " + this.Box);
            Console.WriteLine("BraceletOrStrapMaterial: " + this.BraceletOrStrapMaterial);
            Console.WriteLine("CaseMaterial: " + this.CaseMaterial);
            Console.WriteLine("CaseNo: " + this.CaseNo);
            Console.WriteLine("Dial Color: " + this.DialColor);
            Console.WriteLine("Dial Stamp: " + this.DialStamp);
            Console.WriteLine("Location: " + this.Location);
            Console.WriteLine("Manufacturer: " + this.Manufacturer);
            Console.WriteLine("Model: " + this.Model);
            Console.WriteLine("Movement: " + this.Movement);
            Console.WriteLine("Movement No: " + this.MovementNo);
            Console.WriteLine("Papers: " + this.Papers);
            Console.WriteLine("Reference No: " + this.ReferenceNo);
            Console.WriteLine("------------------------------------------------------------------------------------");
        }
        public override bool Equals(object obj)
        {
            if (obj is WatchesModel otherModel)
            {
                return
                    string.Equals(this.AuctionID, otherModel.AuctionID) &&
                    string.Equals(this.URL, otherModel.URL) &&
                    string.Equals(this.Condition, otherModel.Condition) &&
                    string.Equals(this.Created, otherModel.Created) &&
                    string.Equals(this.Currency, otherModel.Currency) &&
                    string.Equals(this.Description, otherModel.Description) &&
                    string.Equals(this.DimensionsRaw, otherModel.DimensionsRaw) &&
                    string.Equals(this.EstimatePriceRaw, otherModel.EstimatePriceRaw) &&
                    this.ID == otherModel.ID &&
                    Nullable.Equals(this.IsUnsold, otherModel.IsUnsold) &&
                    Nullable.Equals(this.IsSold, otherModel.IsSold) &&
                    string.Equals(this.PhotosRaw, otherModel.PhotosRaw) &&
                    string.Equals(this.PriceRaw, otherModel.PriceRaw) &&
                    string.Equals(this.StartingBid, otherModel.StartingBid) &&
                    string.Equals(this.Title, otherModel.Title) &&
                    string.Equals(this.Box, otherModel.Box) &&
                    string.Equals(this.BraceletOrStrapMaterial, otherModel.BraceletOrStrapMaterial) &&
                    string.Equals(this.CaseMaterial, otherModel.CaseMaterial) &&
                    string.Equals(this.CaseNo, otherModel.CaseNo) &&
                    string.Equals(this.DialColor, otherModel.DialColor) &&
                    string.Equals(this.DialStamp, otherModel.DialStamp) &&
                    string.Equals(this.Location, otherModel.Location) &&
                    string.Equals(this.Manufacturer, otherModel.Manufacturer) &&
                    string.Equals(this.Model, otherModel.Model) &&
                    string.Equals(this.Movement, otherModel.Movement) &&
                    string.Equals(this.MovementNo, otherModel.MovementNo) &&
                    string.Equals(this.Papers, otherModel.Papers) &&
                    string.Equals(this.ReferenceNo, otherModel.ReferenceNo) &&
                    string.Equals(this.Width, otherModel.Width) &&
                    string.Equals(this.Height, otherModel.Height) &&
                    string.Equals(this.Depth, otherModel.Depth);
            }

            return false;
        }
    }


}
