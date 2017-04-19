using Auction.Models;
using System;
using System.Linq.Expressions;

namespace Auction.Web.Models
{
    public class OfferBidsHistoryViewModel
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string User { get; set; }
        public DateTime Created { get; set; }

        public static Expression<Func<Bid, OfferBidsHistoryViewModel>> Create
        {
            get
            {
                return x => new OfferBidsHistoryViewModel
                {
                    Id = x.Id,
                    Price = x.Price,
                    User = x.User.UserName,
                    Created = x.Created
                };
            }
        }
    }
}