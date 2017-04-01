using Auction.Data;
using Auction.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Auction.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        private IAuctionData data;
        private User userProfile;

        protected BaseController(IAuctionData data)
        {
            this.data = data;
        }

        protected BaseController(IAuctionData data, User userProfile)
            : this(data)
        {
            this.userProfile = userProfile;
        }

        protected BaseController()
            : this(new AuctionData(new AuctionContext()))
        {
        }

        public IAuctionData Data
        {
            get { return this.data; }
            private set { this.data = value; }
        }

        public User UserProfile
        {
            get { return this.userProfile; }
            private set { this.userProfile = value; }
        }

        public bool isAdmin()
        {
            if (this.userProfile != null && this.User.IsInRole("Administrator"))
            {
                return true;
            }

            return false;
        }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var username = requestContext.HttpContext.User.Identity.Name;
                var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

                this.userProfile = user;
            }

            return base.BeginExecute(requestContext, callback, state);
        }
    }
}