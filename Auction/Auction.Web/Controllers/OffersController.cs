using Auction.Models;
using Auction.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auction.Web.Controllers
{
    public class OffersController : BaseController
    {
        [HttpGet]
        public ActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult Add(AddOfferViewModel model)
        {
            var time = new DateTime(0, 0, model.Days, model.Hours, model.Minutes, 0);
            var category = this.Data.Categories.All().Where(x => x.Name == model.CategoryName).FirstOrDefault();

            var offer = new Offer()
            {
                Name = model.Name,
                Description = model.Description,
                StartPrice = model.StartPrice,
                CurrentPrice = model.StartPrice,
                StartTime = time,
                TimeLeft = time,
                IsOpen = true,
                Photo = model.Photo,
                Owner = UserProfile,
                Category = category
            };

            this.Data.Offers.Add(offer);
            this.Data.SaveChanges();

            return this.RedirectToAction("Index", "Home");
        }
    }
}