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
        
        public ActionResult Users(string searchString = default(string))
        {
            var users = this.Data.Users.All().Select(UsersViewModel.Create);

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.Email.Contains(searchString) || u.Username.Contains(searchString));
            }

            return this.View(users);
        }
    }
}