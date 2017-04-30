using Auction.Models;
using Auction.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using Auction.Data;

namespace Auction.Web.Controllers
{
    public class BidsController : BaseController
    {
        public BidsController() : base() { }
        public BidsController(IAuctionData data) : base(data){ }
        public BidsController(IAuctionData data, User userProfile) : base(data, userProfile) { }

        [HttpGet]
        public ActionResult Add(int id)
        {
            if(this.UserProfile == null)
            {
                return this.Redirect("/Offers/Index");
            }

            var offer = this.Data.Offers.Find(id);
            if(offer == null)
            {
                return this.Redirect("/Offers/Index");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Add(int id, AddBidViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            if (this.UserProfile == null)
            {
                return this.Redirect("/Offers/Index");
            }

            var offer = this.Data.Offers.Find(id);
            if (offer == null)
            {
                return this.Redirect("/Offers/Index");
            }

            if (model.Price <= offer.CurrentPrice)
            {
                ModelState.AddModelError("", "Invalid bid.");
                return View(model);
            }

            var bid = new Bid()
            {
                Created = DateTime.Now,
                Price = model.Price,
                Offer = offer,
                User = this.UserProfile
            };

            this.Data.Bids.Add(bid);
            offer.CurrentPrice = bid.Price;
            offer.Buyer = this.UserProfile;
            this.Data.SaveChanges();

            return this.Redirect($"/Offers/Details/{id}");
        }

        [HttpGet]
        public ActionResult BidsHistory(int id)
        {
            var bids = this.Data.Bids
                .All()
                .Where(x => x.Offer.Id == id)
                .OrderBy(x => x.Created)
                .Select(OfferBidsHistoryViewModel.Create)
                .AsEnumerable();

            return this.PartialView("_OfferBidsHistory", bids);
        }

        [HttpGet]
        public ActionResult BidsView(string id)
        {
            if (!isAdmin())
            {
                return this.Redirect("/Offers/Index");
            }

            var bids = this.Data.Bids
                        .All()
                        .Where(x => x.User.Id == id)
                        .Select(BidsViewModel.Create);
            
            return this.View(bids);
        }
    }
}