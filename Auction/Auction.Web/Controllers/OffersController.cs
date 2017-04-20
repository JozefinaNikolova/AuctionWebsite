﻿using Auction.Models;
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
        public ActionResult Index(int? id, string searchString = default(string))
        {
            var categoryOffers = this.Data.Offers
                .All()
                .Where(x => x.IsOpen && x.isApproved);

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
                .Where(x => x.IsOpen && x.Owner.UserName == UserProfile.UserName)
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
                .Where(x => !(x.IsOpen) && x.Buyer.UserName == UserProfile.UserName)
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
                IsOpen = true,
                isApproved = false,
                Photo = fileArr,
                Owner = UserProfile,
                Category = category
            };

            this.Data.Offers.Add(offer);
            this.Data.SaveChanges();

            return this.RedirectToAction("Index", "Offers");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var offer = this.Data.Offers
                .All()
                .Where(x => x.Id == id)
                .Select(DetailsOfferViewModel.Create)
                .FirstOrDefault();

            return this.View(offer);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var a = this.UserProfile;
            var offer = this.Data.Offers
                .All()
                .Where(x => x.Id == id)
                .Select(EditOfferViewModel.Create)
                .FirstOrDefault();

            if (this.UserProfile == null)
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

            if (this.UserProfile.Id != model.UserId && !isAdmin())
            {
                return this.Redirect("/Offers/Index");
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

            var category = this.Data.Categories.All()
                .Where(x => x.Name == model.CategoryName)
                .FirstOrDefault();

            var offer = this.Data.Offers.Find(model.Id);
            offer.Name = model.Name;
            offer.Description = model.Description;
            if(file != null)
            {
                offer.Photo = fileArr;
            }

            offer.EndTime = model.EndTime;
            offer.Category = category;

            this.Data.SaveChanges();

            return this.RedirectToAction("Index", "Offers");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (this.UserProfile == null)
            {
                return this.Redirect("/Offers/Index");
            }

            var ownerId = this.Data.Offers.Find(id).Owner.Id;
            if (this.UserProfile.Id != ownerId && !isAdmin())
            {
                return this.Redirect("/Offers/Index");
            }

            this.Data.Offers.Delete(id);
            this.Data.SaveChanges();

            return this.Redirect("/Offers/Index");
        }
    }
}