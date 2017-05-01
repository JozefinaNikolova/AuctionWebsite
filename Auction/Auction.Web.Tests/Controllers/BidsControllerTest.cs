using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Auction.Web.Controllers;
using Moq;
using Auction.Data;
using Auction.Data.Repositories;
using Auction.Models;
using Auction.Web.Models;
using TestStack.FluentMVCTesting;

namespace Auction.Web.Tests.Controllers
{
    [TestClass]
    public class BidsControllerTest
    {
        private User user;
        private Mock<IRepository<Offer>> mockedOfferRepo;
        private Mock<IRepository<Bid>> mockedBidsRepo;
        private Mock<IAuctionData> mockedContext;

        [TestInitialize]
        public void Init()
        {
            user = new User() { Id = "1", UserName = "Fake User", PasswordHash = "1234", FullName = "Test" };

            mockedOfferRepo = new Mock<IRepository<Offer>>();
            mockedBidsRepo = new Mock<IRepository<Bid>>();
            
            mockedContext = new Mock<IAuctionData>();
            mockedContext.Setup(c => c.Offers).Returns(mockedOfferRepo.Object);
            mockedContext.Setup(c => c.Bids).Returns(mockedBidsRepo.Object);
        }

        [TestMethod]
        public void Add_SuccessfullyAddsBid()
        {
            //Arrange
            var fakeOffer = new Offer()
            {
                Id = 1,
                Name = "Test Offer",
                StartPrice = 1,
                CurrentPrice = 1,
                EndTime = DateTime.Now.AddDays(1)
            };

            var bid = new AddBidViewModel()
            {
                Id = 1,
                Price = 2
            };

            var bids = new List<Bid>();

            // Setup repositories
            mockedOfferRepo.Setup(r => r.Find(It.IsAny<int>())).Returns(fakeOffer);
            mockedBidsRepo.Setup(r => r.Add(It.IsAny<Bid>()))
            .Callback<Bid>(b => bids.Add(b));
            
            // Setup controller
            var controller = new BidsController(mockedContext.Object, user);

            // Controller should redirect and not add the bid
            controller.WithCallTo(x => x.Add(1, bid))
                .ShouldRedirectTo($"/Offers/Details/{fakeOffer.Id}");

            Assert.AreEqual(1, bids.Count());
        }

        [TestMethod]
        public void Add_DoesntAddBid_NotLoggedIn()
        {
            //Arrange
            var fakeOffer = new Offer()
            {
                Id = 1,
                Name = "Test Offer",
                StartPrice = 1,
                CurrentPrice = 1,
                EndTime = DateTime.Now.AddDays(1)
            };

            var bid = new AddBidViewModel()
            {
                Id = 1,
                Price = 2
            };

            var bids = new List<Bid>();

            // Setup repositories
            mockedOfferRepo.Setup(r => r.Find(It.IsAny<int>())).Returns(fakeOffer);
            mockedBidsRepo.Setup(r => r.Add(It.IsAny<Bid>()))
            .Callback<Bid>(b => bids.Add(b));
            
            // Setup controller
            var controller = new BidsController(mockedContext.Object);
            
            // Controller should redirect and not add the bid
            controller.WithCallTo(x => x.Add(1, bid))
                .ShouldRedirectTo("/Offers/Index");

            Assert.AreEqual(0, bids.Count());
        }

        [TestMethod]
        public void Add_DoesntAddBid_InvalidOfferId()
        {
            //Arrange
            var bid = new AddBidViewModel()
            {
                Id = 1,
                Price = 2
            };

            var bids = new List<Bid>();

            // Setup repositories
            mockedOfferRepo.Setup(r => r.Find(It.IsAny<int>())).Returns<IEnumerable<Offer>>(null);
            mockedBidsRepo.Setup(r => r.Add(It.IsAny<Bid>()))
            .Callback<Bid>(b => bids.Add(b));
            
            // Setup controller
            var controller = new BidsController(mockedContext.Object, user);

            // Controller should redirect and not add the bid
            controller.WithCallTo(x => x.Add(999, bid))
                .ShouldRedirectTo("/Offers/Index");

            Assert.AreEqual(0, bids.Count());
        }

        [TestMethod]
        public void Add_DoesntAddBid_LowerThanCurrentPrice()
        {
            //Arrange
            var fakeOffer = new Offer()
            {
                Id = 1,
                Name = "Test Offer",
                StartPrice = 1,
                CurrentPrice = 2,
                EndTime = DateTime.Now.AddDays(1)
            };

            var bid = new AddBidViewModel()
            {
                Id = 1,
                Price = 2
            };

            var bids = new List<Bid>();

            // Setup repositories
            mockedOfferRepo.Setup(r => r.Find(It.IsAny<int>())).Returns(fakeOffer);
            mockedBidsRepo.Setup(r => r.Add(It.IsAny<Bid>()))
            .Callback<Bid>(b => bids.Add(b));

            // Setup controller
            var controller = new BidsController(mockedContext.Object, user);

            // Controller should redirect to same view and not add the bid
            controller.WithCallTo(x => x.Add(1, bid))
                .ShouldRenderDefaultView()
                .WithModel<AddBidViewModel>();

            Assert.AreEqual(0, bids.Count());
        }

        [TestMethod]
        public void BidsHistory_ShouldReturnCorrectPartialView()
        {
            // Arrange
            var bids = new List<Bid>();
            bids.Add(new Bid { Id = 1, Price = 2 });
            bids.Add(new Bid { Id = 2, Price = 3 });

            // Setup repository
            mockedBidsRepo.Setup(r => r.All()).Returns(bids.AsQueryable());

            // Setup controller
            var controller = new BidsController(mockedContext.Object, user);

            // Controller should render correct partial view with model
            controller.WithCallTo(x => x.BidsHistory(1))
                .ShouldRenderPartialView("_OfferBidsHistory")
                .WithModel<IEnumerable<OfferBidsHistoryViewModel>>();
        }
    }
}
