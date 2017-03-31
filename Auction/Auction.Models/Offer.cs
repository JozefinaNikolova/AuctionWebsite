using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auction.Models
{
    public class Offer
    {
        private ICollection<Bid> bids;

        public Offer()
        {
            this.bids = new HashSet<Bid>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal StartPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime TimeLeft { get; set; }
        public bool IsOpen { get; set; }
        public byte[] Photo { get; set; }
        public User Owner { get; set; }
        public User Buyer { get; set; }
        public Category Category { get; set; }
        public ICollection<Bid> Bids
        {
            get { return this.bids; }
            set { this.bids = value; }
        }
    }
}
