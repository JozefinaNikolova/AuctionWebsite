using Auction.Models;
using System;
using System.Linq.Expressions;

namespace Auction.Web.Areas.Administrator.Models
{
    public class ApproveOffersViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal CurrentPrice { get; set; }
        public DateTime EndTime { get; set; }
        public byte[] Photo { get; set; }

        public static Expression<Func<Offer, ApproveOffersViewModel>> Create
        {
            get
            {
                return x => new ApproveOffersViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    CurrentPrice = x.CurrentPrice,
                    EndTime = x.EndTime,
                    Photo = x.Photo
                };
            }
        }
    }
}