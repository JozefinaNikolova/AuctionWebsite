using System;
using System.ComponentModel.DataAnnotations;

namespace Auction.Web.Models
{
    public class AddOfferViewModel
    {
        [Required(ErrorMessage = "Please enter offer name")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Please enter price")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public decimal StartPrice { get; set; }
        [Required(ErrorMessage = "Please enter days")]
        public int Days { get; set; }
        [Required(ErrorMessage = "Please enter hours")]
        public int Hours { get; set; }
        [Required(ErrorMessage = "Please enter minutes")]
        public int Minutes { get; set; }
        [Required(ErrorMessage = "Please enter category name")]
        public string CategoryName { get; set; }
    }
}