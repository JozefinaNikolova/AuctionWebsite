using Auction.Data;
using Auction.Models;
using Auction.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auction.Web.Controllers
{
    public class OffersController : BaseController
    {
        public OffersController() : base() { }
        public OffersController(IAuctionData data) : base(data){ }
        public OffersController(IAuctionData data, User userProfile) : base(data, userProfile) { }

        public ActionResult Index(int? id, string searchString = default(string))
        {
            var categoryOffers = this.Data.Offers
                .All()
                .Where(x => x.EndTime >= DateTime.Now && x.isApproved);

            if (id != null)
            {
                categoryOffers = categoryOffers
                    .Where(x => x.Category.Id == id);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                categoryOffers = categoryOffers.Where(x => x.Name.Contains(searchString));
            }

            var offers = categoryOffers.Select(AllOffersViewModel.Create);

            var categories = this.Data.Categories.All().Select(CategoryViewModel.Create).AsEnumerable();
            var vm = new OffersCategoriesViewModel(offers, categories);

            return View(vm);
        }

        public ActionResult My()
        {
            if(this.UserProfile == null)
            {
                return this.Redirect("/Offers/Index");
            }

            var offers = this.Data.Offers
                .All()
                .Where(x => x.EndTime >= DateTime.Now && x.Owner.UserName == UserProfile.UserName)
                .Select(AllOffersViewModel.Create);

            return View(offers);
        }

        public ActionResult WonBids()
        {
            if (this.UserProfile == null)
            {
                return this.Redirect("/Offers/Index");
            }

            var offers = this.Data.Offers
                .All()
                .Where(x => x.EndTime < DateTime.Now && x.Buyer.UserName == UserProfile.UserName)
                .Select(AllOffersViewModel.Create);

            return View(offers);
        }

        [HttpGet]
        public ActionResult Add()
        {
            if (this.UserProfile == null)
            {
                return this.Redirect("/Offers/Index");
            }

            return this.View();
        }

        [HttpPost]
        public ActionResult Add(AddOfferViewModel model, HttpPostedFileBase file)
        {
            if (this.UserProfile == null)
            {
                return this.Redirect("/Offers/Index");
            }

            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            bool categoryExists = true;
            if(model.CategoryName == null)
            {
                categoryExists = false;
            }
            else
            {
                categoryExists = this.Data.Categories.All().Any(x => x.Name == model.CategoryName);
            }

            if(!categoryExists)
            {
                ModelState.AddModelError("", "Invalid category.");
                return View(model);
            }

            int length = 0;
            if(file != null)
            {
                length = file.ContentLength;
            }

            var fileArr = new byte[length];

            if (file != null)
            {
                string pic = Path.GetFileName(file.FileName);
                
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    fileArr = ms.GetBuffer();
                }
            }

            var time = DateTime.Now
                .AddDays(model.Days)
                .AddHours(model.Hours)
                .AddMinutes(model.Minutes);

            var category = this.Data.Categories.All()
                .Where(x => x.Name == model.CategoryName)
                .FirstOrDefault();

            var offer = new Offer()
            {
                Name = model.Name,
                Description = model.Description,
                StartPrice = model.StartPrice,
                CurrentPrice = model.StartPrice,
                EndTime = time,
                isApproved = false,
                Photo = fileArr,
                Owner = UserProfile,
                Category = category
            };

            this.Data.Offers.Add(offer);
            this.Data.SaveChanges();

            return this.Redirect("/Offers/Index");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var offer = this.Data.Offers
                .All()
                .Where(x => x.Id == id)
                .Select(DetailsOfferViewModel.Create)
                .FirstOrDefault();

            if(offer == null)
            {
                return this.Redirect("/Offers/Index");
            }

            return this.View(offer);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (this.UserProfile == null)
            {
                return this.Redirect("/Offers/Index");
            }

            var offer = this.Data.Offers
                .All()
                .Where(x => x.Id == id)
                .Select(EditOfferViewModel.Create)
                .FirstOrDefault();

            if(offer == null)
            {
                return this.Redirect("/Offers/Index");
            }

            if(this.UserProfile.Id != offer.UserId && !isAdmin())
            {
                return this.Redirect("/Offers/Index");
            }

            return this.View(offer);
        }

        [HttpPost]
        public ActionResult Edit(EditOfferViewModel model, HttpPostedFileBase file)
        {
            if (this.UserProfile == null)
            {
                return this.Redirect("/Offers/Index");
            }

            var offer = this.Data.Offers.Find(model.Id);

            if (this.UserProfile.Id != offer.Owner.Id && !isAdmin())
            {
                return this.Redirect("/Offers/Index");
            }

            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var category = this.Data.Categories.All()
                .Where(x => x.Name == model.CategoryName)
                .FirstOrDefault();

            if(category == null)
            {
                ModelState.AddModelError("", "Invalid category.");
                return View(model);
            }
            
            int length = 0;
            if (file != null)
            {
                length = file.ContentLength;
            }

            var fileArr = new byte[length];

            if (file != null)
            {
                string pic = Path.GetFileName(file.FileName);

                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    fileArr = ms.GetBuffer();
                }
            }

            offer.Name = model.Name;
            offer.Description = model.Description;
            if(file != null)
            {
                offer.Photo = fileArr;
            }

            offer.EndTime = model.EndTime;
            offer.Category = category;

            this.Data.SaveChanges();

            return this.Redirect("/Offers/Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (this.UserProfile == null)
            {
                return this.Redirect("/Offers/Index");
            }
            var offer = this.Data.Offers.Find(id);

            if(offer == null)
            {
                return this.Redirect("/Offers/Index");
            }

            var ownerId = offer.Owner.Id;

            if (this.UserProfile.Id != ownerId && !isAdmin())
            {
                return this.Redirect("/Offers/Index");
            }

            var bidIds = offer.Bids.Select(x => x.Id);

            foreach (var bidId in bidIds.ToList())
            {
                this.Data.Bids.Delete(bidId);
            }

            this.Data.Offers.Delete(id);
            this.Data.SaveChanges();

            return this.Redirect("/Offers/Index");
        }
    }
}