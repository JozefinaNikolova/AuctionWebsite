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
        public DateTime EndTime { get; set; }
        public bool isApproved { get; set; }
        public bool IsOpen { get; set; }
        public byte[] Photo { get; set; }
        public virtual User Owner { get; set; }
        public virtual User Buyer { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Bid> Bids
        {
            get { return this.bids; }
            set { this.bids = value; }
        }
    }
}
