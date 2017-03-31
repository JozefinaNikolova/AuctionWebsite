using System;

namespace Auction.Models
{
    public class Bid
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime Time { get; set; }
        public virtual Offer Offer { get; set; }
        public virtual User User { get; set; }
    }
}
