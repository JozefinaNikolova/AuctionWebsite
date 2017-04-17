using System.Collections.Generic;

namespace Auction.Web.Models
{
    public class OffersCategoriesViewModel
    {
        public OffersCategoriesViewModel(IEnumerable<AllOffersViewModel> offers, IEnumerable<string> categories)
        {
            this.AllOffersViewModel = offers;
            this.Categories = categories;
        }

        public IEnumerable<AllOffersViewModel> AllOffersViewModel { get; set; }
        public IEnumerable<string> Categories { get; set; }
    }
}