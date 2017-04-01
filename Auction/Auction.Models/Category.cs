using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auction.Models
{
    public class Category
    {
        private ICollection<Offer> offers;

        public Category()
        {
            this.offers = new HashSet<Offer>();
        }
        
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<Offer> Offers
        {
            get { return this.offers; }
            set { this.offers = value; }
        }
    }
}
