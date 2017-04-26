using System.ComponentModel.DataAnnotations;
namespace Auction.Web.Models
{
    public class AddCategoryViewModel
    {
        [Required(ErrorMessage = "Please enter category name")]
        [MaxLength(15)]
        public string Name { get; set; }
    }
}