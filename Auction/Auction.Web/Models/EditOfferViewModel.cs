using Auction.Models;
using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Auction.Web.Models
{
    public class EditOfferViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [DisplayName("End Time")]
        public DateTime EndTime { get; set; }
        public byte[] Photo { get; set; }
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