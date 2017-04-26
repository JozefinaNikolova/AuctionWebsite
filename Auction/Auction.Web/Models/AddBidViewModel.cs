using System.ComponentModel.DataAnnotations;

namespace Auction.Web.Models
{
    public class AddBidViewModel
    {
        public int Id { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}