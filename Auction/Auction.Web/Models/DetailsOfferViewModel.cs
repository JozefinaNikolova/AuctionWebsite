using Auction.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Auction.Web.Models
{
    public class DetailsOfferViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal StartPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public DateTime EndTime { get; set; }
        public byte[] Photo { get; set; }
        public bool IsOpen { get; set; }
        public string OwnerId { get; set; }
        public string OwnerPhone { get; set; }
        public string OwnerEmail { get; set; }
        public string OwnerName { get; set; }
        public string BuyerName { get; set; }
        public string CategoryName { get; set; }
        public ICollection<Bid> Bids { get; set; }

        public static Expression<Func<Offer, DetailsOfferViewModel>> Create
        {
            get
            {
                return x => new DetailsOfferViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    StartPrice = x.StartPrice,
                    CurrentPrice = x.CurrentPrice,
                    EndTime = x.EndTime,
                    Photo = x.Photo,
                    IsOpen = x.IsOpen,
                    OwnerId = x.Owner.Id,
                    OwnerPhone = x.Owner.PhoneNumber, 
                    OwnerEmail = x.Owner.Email,
                    OwnerName = x.Owner.FullName,
                    BuyerName = x.Buyer.FullName,
                    CategoryName = x.Category.Name,
                    Bids = x.Bids
                };
            }
        }
    }
}