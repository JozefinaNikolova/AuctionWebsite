using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Auction.Models
{
    public class User : IdentityUser
    {
        private ICollection<Offer> ownOffers;
        private ICollection<Offer> boughtOffers;
        private ICollection<Bid> bids;

        public User()
        {
            this.ownOffers = new HashSet<Offer>();
            this.boughtOffers = new HashSet<Offer>();
            this.bids = new HashSet<Bid>();
        }

        [Required]
        public string FullName { get; set; }
        public string Address { get; set; }
        public ICollection<Offer> OwnOffers
        {
            get { return this.ownOffers; }
            set { this.ownOffers = value; }
        }
        public ICollection<Offer> BoughtOffers
        {
            get { return this.boughtOffers; }
            set { this.boughtOffers = value; }
        }
        public ICollection<Bid> Bids
        {
            get { return this.bids; }
            set { this.bids = value; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
