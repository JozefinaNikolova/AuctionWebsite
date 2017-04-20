using Auction.Web.Areas.Administrator.Models;
using Auction.Web.Controllers;
using System.Linq;
using System.Web.Mvc;

namespace Auction.Web.Areas.Administrator.Controllers
{
    public class AdminController : BaseController
    {
        [HttpGet]
        public ActionResult Approve()
        {
            var offers = this.Data.Offers
                .All()
                .Where(x => !x.isApproved)
                .Select(ApproveOffersViewModel.Create);

            return View(offers);
        }

        [HttpGet]
        public ActionResult ApproveOffer(int id)
        {
            var offer = this.Data.Offers.Find(id);
            offer.isApproved = true;
            this.Data.SaveChanges();

            return this.Redirect("/Admin/Approve");
        }

        [HttpGet]
        public ActionResult DeleteOffer(int id)
        {
            var offer = this.Data.Offers.Delete(id);
            this.Data.SaveChanges();

            return this.Redirect("/Admin/Approve");
        }
    }
}