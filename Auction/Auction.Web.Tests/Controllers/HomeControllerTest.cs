using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Auction.Web;
using Auction.Web.Controllers;

namespace Auction.Web.Tests.Controllers
{
    [TestClass]
    public class OffersControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            OffersController controller = new OffersController();

            //// Act
            //ViewResult result = controller.Index() as ViewResult;

            //// Assert
            //Assert.IsNotNull(result);
        }
    }
}
