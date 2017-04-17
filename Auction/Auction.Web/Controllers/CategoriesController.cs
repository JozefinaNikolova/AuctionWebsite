using Auction.Models;
using Auction.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auction.Web.Controllers
{
    public class CategoriesController : BaseController
    {
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(AddCategoryViewModel model)
        {
            var category = new Category
            {
                Name = model.Name
            };

            this.Data.Categories.Add(category);
            this.Data.SaveChanges();

            return this.Redirect("/Offers/Index");
        }
    }
}