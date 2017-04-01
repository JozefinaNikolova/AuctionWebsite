using System;
using System.ComponentModel.DataAnnotations;

namespace Auction.Web.Models
{
    public class AddOfferViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public decimal StartPrice { get; set; }
        public int Days { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public string CategoryName { get; set; }
    }
}