using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IneichenTask
{
    class AuctionModel
    {
        public string Title { get; set; }
        public string ImageURL { get; set; }
        public string WatchesPageURL { get; set; }
        public string AuctionID { get; set; }
        public string DateTime { get; set; }
        public string Location { get; set; }

        public void PrintData()
        {
            Console.WriteLine("Title: " + Title);
            Console.WriteLine("Image URL: " + ImageURL);
            Console.WriteLine("Watches Page URL: " + WatchesPageURL);
            Console.WriteLine("Auctions ID: " + AuctionID);
            Console.WriteLine("Date time: " + DateTime);
            Console.WriteLine("Location: "+Location);
            Console.WriteLine("-----------------------------------------------------------------------------------");
        }

        public bool Equals(AuctionModel model)
        {
            return model.Title.Equals(this.Title)
                && model.ImageURL.Equals(this.ImageURL)
                && model.WatchesPageURL.Equals(this.WatchesPageURL)
                && model.DateTime.Equals(this.DateTime)
                && model.Location.Equals(this.Location);
        }
    }
}
