using Auction.Models;
using Auction.Web.Models;
using System.Linq;
using System.Web.Mvc;

namespace Auction.Web.Controllers
{
    public class CategoriesController : BaseController
    {
        [HttpGet]
        public ActionResult Add()
        {
            if(!isAdmin())
            {
                return this.Redirect("/Offers/Index");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Add(AddCategoryViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            if (!isAdmin())
            {
                return this.Redirect("/Offers/Index");
            }

            bool categoryExists = this.Data.Categories.All().Any(x => x.Name == model.Name);

            if(categoryExists)
            {
                ModelState.AddModelError("", "Category already exists.");
                return View(model);
            }

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