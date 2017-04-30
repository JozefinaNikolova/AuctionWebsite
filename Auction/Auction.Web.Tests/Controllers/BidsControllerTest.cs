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

namespace Auction.Web.Tests.Controllers
{
    [TestClass]
    public class BidsControllerTest
    {
        [TestMethod]
        public void Add_SuccessfullyAddsBid()
        {
            //Arrange
            var user = new User() { Id = "1", UserName = "Fake User", PasswordHash = "1234" , FullName = "Test"};
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
            var mockedOfferRepo = new Mock<IRepository<Offer>>();
            mockedOfferRepo.Setup(r => r.Find(It.IsAny<int>())).Returns(fakeOffer);
            var mockedBidsRepo = new Mock<IRepository<Bid>>();
            mockedBidsRepo.Setup(r => r.Add(It.IsAny<Bid>()))
            .Callback<Bid>(b => bids.Add(b));

            // Setup data layer
            var mockedContext = new Mock<IAuctionData>();
            mockedContext.Setup(c => c.Offers).Returns(mockedOfferRepo.Object);
            mockedContext.Setup(c => c.Bids).Returns(mockedBidsRepo.Object);


            // Setup controller
            var controller = new BidsController(mockedContext.Object, user);

            //Act
            controller.Add(1, bid);
            
            //Assert
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
            var mockedOfferRepo = new Mock<IRepository<Offer>>();
            mockedOfferRepo.Setup(r => r.Find(It.IsAny<int>())).Returns(fakeOffer);
            var mockedBidsRepo = new Mock<IRepository<Bid>>();
            mockedBidsRepo.Setup(r => r.Add(It.IsAny<Bid>()))
            .Callback<Bid>(b => bids.Add(b));

            // Setup data layer
            var mockedContext = new Mock<IAuctionData>();
            mockedContext.Setup(c => c.Offers).Returns(mockedOfferRepo.Object);
            mockedContext.Setup(c => c.Bids).Returns(mockedBidsRepo.Object);


            // Setup controller
            var controller = new BidsController(mockedContext.Object);

            //Act
            controller.Add(1, bid);

            //Assert
            Assert.AreEqual(0, bids.Count());
        }

        [TestMethod]
        public void Add_DoesntAddBid_InvalidOfferId()
        {
            //Arrange
            var user = new User() { Id = "1", UserName = "Fake User", PasswordHash = "1234", FullName = "Test" };

            var bid = new AddBidViewModel()
            {
                Id = 1,
                Price = 2
            };

            var bids = new List<Bid>();

            // Setup repositories
            var mockedOfferRepo = new Mock<IRepository<Offer>>();
            mockedOfferRepo.Setup(r => r.Find(It.IsAny<int>())).Returns<IEnumerable<Offer>>(null);
            var mockedBidsRepo = new Mock<IRepository<Bid>>();
            mockedBidsRepo.Setup(r => r.Add(It.IsAny<Bid>()))
            .Callback<Bid>(b => bids.Add(b));

            // Setup data layer
            var mockedContext = new Mock<IAuctionData>();
            mockedContext.Setup(c => c.Offers).Returns(mockedOfferRepo.Object);
            mockedContext.Setup(c => c.Bids).Returns(mockedBidsRepo.Object);


            // Setup controller
            var controller = new BidsController(mockedContext.Object, user);

            //Act
            controller.Add(999, bid);

            //Assert
            Assert.AreEqual(0, bids.Count());
        }

        [TestMethod]
        public void Add_DoesntAddBid_LowerThanCurrentPrice()
        {
            //Arrange
            var user = new User() { Id = "1", UserName = "Fake User", PasswordHash = "1234", FullName = "Test" };
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
            var mockedOfferRepo = new Mock<IRepository<Offer>>();
            mockedOfferRepo.Setup(r => r.Find(It.IsAny<int>())).Returns(fakeOffer);
            var mockedBidsRepo = new Mock<IRepository<Bid>>();
            mockedBidsRepo.Setup(r => r.Add(It.IsAny<Bid>()))
            .Callback<Bid>(b => bids.Add(b));

            // Setup data layer
            var mockedContext = new Mock<IAuctionData>();
            mockedContext.Setup(c => c.Offers).Returns(mockedOfferRepo.Object);
            mockedContext.Setup(c => c.Bids).Returns(mockedBidsRepo.Object);


            // Setup controller
            var controller = new BidsController(mockedContext.Object, user);

            //Act
            controller.Add(1, bid);

            //Assert
            Assert.AreEqual(0, bids.Count());
        }
    }
}
