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
        public ActionResult Index()
        {
            var offers = this.Data.Offers
                .All()
                .Where(x => x.IsOpen)
                .Select(AllOffersViewModel.Create);

            return View(offers);
        }

        public ActionResult My()
        {
            var offers = this.Data.Offers
                .All()
                .Where(x => !(x.IsOpen) && x.Owner.UserName == UserProfile.UserName)
                .Select(AllOffersViewModel.Create);

            return View(offers);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult Add(AddOfferViewModel model, HttpPostedFileBase file)
        {
            var fileArr = new byte[file.ContentLength];

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
        public ActionResult Delete(int id)
        {
            this.Data.Offers.Delete(id);
            this.Data.SaveChanges();

            return this.Redirect("/Offers/Index");
        }
    }
}