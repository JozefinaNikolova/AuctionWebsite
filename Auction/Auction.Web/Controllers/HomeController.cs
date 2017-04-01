using Auction.Data;
using Auction.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auction.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var offers = this.Data.Offers
                .All()
                .Where(x => x.IsOpen)
                .Select(AllOffersViewModel.Create);

            return View(offers);
        }
    }
}