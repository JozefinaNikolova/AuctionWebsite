using Auction.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Auction.Web.Models
{
    public class DetailsOfferViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [DisplayName("Start Price")]
        public decimal StartPrice { get; set; }
        [DisplayName("Current Price")]
        public decimal CurrentPrice { get; set; }
        [DisplayName("End Time")]
        public DateTime EndTime { get; set; }
        public byte[] Photo { get; set; }
        public bool IsOpen { get; set; }
        public string OwnerId { get; set; }
        [DisplayName("Owner Phone Number")]
        public string OwnerPhone { get; set; }
        [DisplayName("Owner Email")]
        public string OwnerEmail { get; set; }
        [DisplayName("Owner Name")]
        public string OwnerName { get; set; }
        public string OwnerUsername { get; set; }
        [DisplayName("Buyer Name")]
        public string BuyerName { get; set; }
        [DisplayName("Category")]
        public string CategoryName { get; set; }
        public IEnumerable<OfferBidsHistoryViewModel> Bids { get; set; }

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
                    OwnerUsername = x.Owner.UserName,
                    BuyerName = x.Buyer.FullName,
                    CategoryName = x.Category.Name,
                    Bids = x.Bids.AsQueryable().Select(OfferBidsHistoryViewModel.Create)
                };
            }
        }
    }
}