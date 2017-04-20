using Auction.Models;
using System;
using System.Linq.Expressions;

namespace Auction.Web.Models
{
    public class BidsViewModel
    {
        public decimal Price { get; set; }
        public DateTime Created { get; set; }
        public int OfferId { get; set; }
        public string OfferName { get; set; }
        public string User { get; set; }

        public static Expression<Func<Bid, BidsViewModel>> Create
        {
            get
            {
                return x => new BidsViewModel
                {
                    Price = x.Price,
                    Created = x.Created,
                    OfferId = x.Offer.Id,
                    OfferName = x.Offer.Name,
                    User = x.User.FullName
                };
            }
        }
    }
}