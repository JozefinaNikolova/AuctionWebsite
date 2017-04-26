using System.ComponentModel.DataAnnotations;

namespace Auction.Web.Models
{
    public class AddBidViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Price is invalid")]
        public decimal Price { get; set; }
    }
}