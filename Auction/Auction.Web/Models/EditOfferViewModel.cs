using Auction.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Auction.Web.Models
{
    public class EditOfferViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter offer name")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Please enter end time")]
        [DisplayName("End Time")]
        public DateTime EndTime { get; set; }
        public byte[] Photo { get; set; }
        [Required(ErrorMessage = "Please enter category name")]
        [DisplayName("Category Name")]
        public string CategoryName { get; set; }
        public string UserId { get; set; }

        public static Expression<Func<Offer, EditOfferViewModel>> Create
        {
            get
            {
                return x => new EditOfferViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    EndTime = x.EndTime,
                    Photo = x.Photo,
                    CategoryName = x.Category.Name,
                    UserId = x.Owner.Id
                };
            }
        }
    }
}