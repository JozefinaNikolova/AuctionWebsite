using System;

namespace Auction.Web.Models
{
    public class AddOfferViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Photo { get; set; }
        public decimal StartPrice { get; set; }
        public int Days { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public string CategoryName { get; set; }
    }
}