using Auction.Web.Areas.Administrator.Controllers;
using System.Web.Mvc;

namespace Auction.Web.Areas.Administrator
{
    public class AdministratorAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Administrator";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Administrator_Admin",
                "Admin/{action}/{id}",
                new { controller = "Admin", action = "Approve", id = UrlParameter.Optional },
                new { isMethodInHomeController = new RootRouteConstraint<AdminController>() }
            );

            context.MapRoute(
                "Administrator_default",
                "Administrator/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}