using System.ComponentModel.DataAnnotations;
namespace Auction.Web.Models
{
    public class AddCategoryViewModel
    {
        [Required]
        [MaxLength(15)]
        public string Name { get; set; }
    }
}