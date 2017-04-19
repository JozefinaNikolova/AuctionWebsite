using Auction.Models;
using Auction.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Auction.Web.Controllers
{
    public class BidsController : BaseController
    {
        [HttpGet]
        public ActionResult Add(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(int id, AddBidViewModel model)
        {
            var offer = this.Data.Offers.Find(id);

            var bid = new Bid()
            {
                Created = DateTime.Now,
                Price = model.Price,
                Offer = offer,
                User = this.UserProfile
            };

            this.Data.Bids.Add(bid);
            offer.CurrentPrice = bid.Price;
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
    }
}