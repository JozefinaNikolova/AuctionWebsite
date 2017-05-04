using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using TestStack.FluentMVCTesting;
using Auction.Data.Repositories;
using Auction.Models;
using Auction.Data;
using Auction.Web.Controllers;
using Auction.Web.Models;
using System.Web.Mvc;
using System.Security.Principal;

namespace Auction.Web.Tests.Controllers
{
    [TestClass]
    public class OffersControllerTest
    {
        private User user;
        private Mock<IRepository<Offer>> mockedOfferRepo;
        private Mock<IRepository<Category>> mockedCategoriesRepo;
        private Mock<IAuctionData> mockedContext;

        [TestInitialize]
        public void Init()
        {
            user = new User() { Id = "1", UserName = "Fake User", PasswordHash = "1234", FullName = "Test" };

            mockedOfferRepo = new Mock<IRepository<Offer>>();
            mockedCategoriesRepo = new Mock<IRepository<Category>>();

            mockedContext = new Mock<IAuctionData>();
            mockedContext.Setup(c => c.Offers).Returns(mockedOfferRepo.Object);
            mockedContext.Setup(c => c.Categories).Returns(mockedCategoriesRepo.Object);
        }

        [TestMethod]
        public void Index_ShouldReturnCorrectView()
        {
            //Arrange
            var offers = new List<Offer>();
            offers.Add(new Offer { Id = 1, Name = "Test", EndTime = DateTime.Now.AddYears(1), isApproved = true });
            var categories = new List<Category>();
            categories.Add(new Category { Id = 1, Name = "Test" });

            // Setup repositories
            mockedOfferRepo.Setup(r => r.All()).Returns(offers.AsQueryable());
            mockedCategoriesRepo.Setup(r => r.All()).Returns(categories.AsQueryable());

            // Setup controller
            var controller = new OffersController(mockedContext.Object);

            // Controller should return correct view with correct model and data
            controller.WithCallTo(x => x.Index(null, null))
                .ShouldRenderDefaultView()
                .WithModel<OffersCategoriesViewModel>
                (x => x.AllOffersViewModel.FirstOrDefault().Name == "Test"
                && x.Categories.FirstOrDefault().Name == "Test");
        }

        [TestMethod]
        public void My_ShouldReturnCorrectView()
        {
            //Arrange
            var notLoggedUser = new User() { Id = "2", UserName = "Not Logged", PasswordHash = "1234", FullName = "Not Logged" };
            var offers = new List<Offer>();
            offers.Add(new Offer { Id = 1, Name = "YesReturn", EndTime = DateTime.Now.AddYears(1), Owner = user });
            offers.Add(new Offer { Id = 2, Name = "NoReturn", EndTime = DateTime.Now.AddYears(1), Owner = notLoggedUser });
            offers.Add(new Offer { Id = 3, Name = "NoReturn", EndTime = DateTime.Now.AddYears(1), Owner = notLoggedUser });

            // Setup repositories
            mockedOfferRepo.Setup(r => r.All()).Returns(offers.AsQueryable());

            // Setup controller
            var controller = new OffersController(mockedContext.Object, user);

            // Controller should return correct view with correct model and data
            controller.WithCallTo(x => x.My())
                .ShouldRenderDefaultView()
                .WithModel<IEnumerable<AllOffersViewModel>>
                (x => x.Count() == 1 && x.FirstOrDefault().Name == "YesReturn");
        }

        [TestMethod]
        public void My_ShouldRedirectIfNotLogged()
        {
            //Arrange
            var offers = new List<Offer>();

            // Setup repositories
            mockedOfferRepo.Setup(r => r.All()).Returns(offers.AsQueryable());

            // Setup controller with no user
            var controller = new OffersController(mockedContext.Object);

            // Controller should redirect
            controller.WithCallTo(x => x.My())
                .ShouldRedirectTo("/Offers/Index");
        }

        [TestMethod]
        public void WonBids_ShouldReturnCorrectView()
        {
            //Arrange
            var notLoggedUser = new User() { Id = "2", UserName = "Not Logged", PasswordHash = "1234", FullName = "Not Logged" };
            var offers = new List<Offer>();
            offers.Add(new Offer { Id = 1, Name = "YesReturn", EndTime = new DateTime(1900, 1, 1), Buyer = user });
            offers.Add(new Offer { Id = 2, Name = "NoReturn", EndTime = new DateTime(1900, 1, 1), Buyer = notLoggedUser });
            offers.Add(new Offer { Id = 3, Name = "NoReturn", EndTime = new DateTime(1900, 1, 1), Buyer = notLoggedUser });

            // Setup repositories
            mockedOfferRepo.Setup(r => r.All()).Returns(offers.AsQueryable());

            // Setup controller
            var controller = new OffersController(mockedContext.Object, user);

            // Controller should return correct view with correct model and data
            controller.WithCallTo(x => x.WonBids())
                .ShouldRenderDefaultView()
                .WithModel<IEnumerable<AllOffersViewModel>>
                (x => x.Count() == 1 && x.FirstOrDefault().Name == "YesReturn");
        }

        [TestMethod]
        public void WonBids_ShouldRedirectIfNotLogged()
        {
            //Arrange
            var offers = new List<Offer>();

            // Setup repositories
            mockedOfferRepo.Setup(r => r.All()).Returns(offers.AsQueryable());

            // Setup controller with no user
            var controller = new OffersController(mockedContext.Object);

            // Controller should redirect
            controller.WithCallTo(x => x.WonBids())
                .ShouldRedirectTo("/Offers/Index");
        }

        [TestMethod]
        public void Add_Get_ShouldReturnCorrectView()
        {
            var controller = new OffersController(mockedContext.Object, user);
            
            controller.WithCallTo(x => x.Add())
                .ShouldRenderDefaultView();
        }

        [TestMethod]
        public void Add_Get_ShouldRedirectIfNotLogged()
        {
            var controller = new OffersController(mockedContext.Object);

            controller.WithCallTo(x => x.Add())
                .ShouldRedirectTo("/Offers/Index");
        }


        [TestMethod]
        public void Add_Post_SuccessfullyAddsOffer()
        {
            //Arrange
            var categories = new List<Category>() { new Category { Id = 1, Name = "Home" } };
            var offer = new AddOfferViewModel()
            {
                Name = "Test",
                StartPrice = 1,
                CategoryName = "Home"
            };

            var addedOffers = new List<Offer>();

            // Setup repositories
            mockedCategoriesRepo.Setup(r => r.All()).Returns(categories.AsQueryable());
            mockedOfferRepo.Setup(r => r.Add(It.IsAny<Offer>()))
           .Callback<Offer>(o => addedOffers.Add(o));

            // Setup controller
            var controller = new OffersController(mockedContext.Object, user);

            // Controller should redirect
            controller.WithCallTo(x => x.Add(offer, null))
                .ShouldRedirectTo("/Offers/Index");

            Assert.AreEqual("Test", addedOffers.FirstOrDefault().Name);
            Assert.AreEqual(1, addedOffers.FirstOrDefault().StartPrice);
            Assert.AreEqual("Home", addedOffers.FirstOrDefault().Category.Name);
        }

        [TestMethod]
        public void Add_Post_ShouldRedirectIfNotLogged()
        {
            //Arrange
            var offer = new AddOfferViewModel();

            // Setup controller with no user
            var controller = new OffersController(mockedContext.Object);

            // Controller should redirect
            controller.WithCallTo(x => x.Add(offer, null))
                .ShouldRedirectTo("/Offers/Index");
        }

        [TestMethod]
        public void Add_Post_ShouldReturnViewIfCategoryDoesntExist()
        {
            //Arrange
            var offer = new AddOfferViewModel();

            // Setup repositories
            mockedCategoriesRepo.Setup(r => r.All()).Returns<IQueryable<Category>>(null);

            // Setup controller
            var controller = new OffersController(mockedContext.Object, user);

            // Controller should return correct view
            controller.WithCallTo(x => x.Add(offer, null))
                .ShouldRenderDefaultView()
                .WithModel<AddOfferViewModel>();
        }

        [TestMethod]
        public void Details_ShouldReturnCorrectView()
        {
            //Arrange
            var offers = new List<Offer>()
            {
                new Offer
                {
                    Id = 1,
                    Name = "Test",
                    Owner = user,
                    Buyer = user,
                    Category = new Category(),
                    Bids = new List<Bid>()
                }
            };

            // Setup repositories
            mockedOfferRepo.Setup(r => r.All()).Returns(offers.AsQueryable());

            // Setup controller
            var controller = new OffersController(mockedContext.Object, user);

            // Controller should return correct view with model and data
            controller.WithCallTo(x => x.Details(1))
                .ShouldRenderDefaultView()
                .WithModel<DetailsOfferViewModel>
                (x => x.Id == 1 && x.Name == "Test");
        }

        [TestMethod]
        public void Details_ShouldRedirectIfOfferDoesntExist()
        {
            // Setup repositories
            mockedCategoriesRepo.Setup(r => r.All()).Returns<IQueryable<Offer>>(null);

            // Setup controller
            var controller = new OffersController(mockedContext.Object, user);

            // Controller should redirect
            controller.WithCallTo(x => x.Details(1))
                .ShouldRedirectTo("/Offers/Index");
        }

        [TestMethod]
        public void Edit_Get_ShouldRedirectIfNotLogged()
        {
            // Setup controller with no user
            var controller = new OffersController(mockedContext.Object);

            // Controller should redirect
            controller.WithCallTo(x => x.Edit(1))
                .ShouldRedirectTo("/Offers/Index");
        }

        [TestMethod]
        public void Edit_Get_ShouldRedirectIfOfferDoesntExist()
        {
            // Setup repositories
            mockedCategoriesRepo.Setup(r => r.All()).Returns<IQueryable<Offer>>(null);

            // Setup controller
            var controller = new OffersController(mockedContext.Object, user);

            // Controller should redirect
            controller.WithCallTo(x => x.Edit(1))
                .ShouldRedirectTo("/Offers/Index");
        }

        [TestMethod]
        public void Edit_Get_ShouldReturnCorrectView()
        {
            // Arrange
            var offers = new List<Offer>()
            {
                new Offer {Id = 1, Name = "Test", Category = new Category(), Owner = user }
            };

            // Setup repositories
            mockedOfferRepo.Setup(r => r.All()).Returns(offers.AsQueryable());

            // Setup context to have user as administrator
            Mock<ControllerContext> mockContext = GetContext(true);

            // Setup controller
            var controller = new OffersController(mockedContext.Object, user)
            {
                ControllerContext = mockContext.Object
            };

            controller.WithCallTo(x => x.Edit(1))
                .ShouldRenderDefaultView()
                .WithModel<EditOfferViewModel>
                (x => x.Id == offers.First().Id && x.Name == offers.First().Name);
        }

        [TestMethod]
        public void Edit_Post_ShouldRedirectIfNotLogged()
        {
            // Arrange
            var offer = new EditOfferViewModel();

            // Setup context to have user as administrator
            Mock<ControllerContext> mockContext = GetContext(true);

            // Setup controller
            var controller = new OffersController(mockedContext.Object, user)
            {
                ControllerContext = mockContext.Object
            };

            // Controller should redirect
            controller.WithCallTo(x => x.Edit(offer, null))
                .ShouldRedirectTo("/Offers/Index");
        }

        [TestMethod]
        public void Edit_Post_ShouldRedirectIfNotOwnerOrAdmin()
        {
            // Arrange
            var notOwnerUser = new User() { Id = "2", UserName = "Fake User", PasswordHash = "1234", FullName = "Test" };
            var offerVM = new EditOfferViewModel();
            var offer = new Offer() { Id = 1, Name = "Test", Owner = user };
            var categories = new List<Category>();
            categories.Add(new Category { Id = 1, Name = "Test" });

            // Setup repositories
            mockedOfferRepo.Setup(r => r.Find(It.IsAny<int>())).Returns(offer);
            mockedCategoriesRepo.Setup(r => r.All()).Returns(categories.AsQueryable());

            // Setup context to have user as not administrator
            Mock<ControllerContext> mockContext = GetContext(false);

            // Setup controller
            var controller = new OffersController(mockedContext.Object, notOwnerUser)
            {
                ControllerContext = mockContext.Object
            };

            // Controller should redirect
            controller.WithCallTo(x => x.Edit(offerVM, null))
                .ShouldRedirectTo("/Offers/Index");
        }

        [TestMethod]
        public void Edit_Post_ShouldReturnViewIfCategoryDoesntExist()
        {
            // Arrange
            var offerVM = new EditOfferViewModel { CategoryName = "Fake"};
            var offer = new Offer() { Id = 1, Name = "Test", Owner = user };
            var categories = new List<Category>();
            categories.Add(new Category { Id = 1, Name = "Test" });

            // Setup repositories
            mockedOfferRepo.Setup(r => r.Find(It.IsAny<int>())).Returns(offer);
            mockedCategoriesRepo.Setup(r => r.All()).Returns(categories.AsQueryable());

            // Setup context to have user as not administrator
            Mock<ControllerContext> mockContext = GetContext(true);

            // Setup controller
            var controller = new OffersController(mockedContext.Object, user)
            {
                ControllerContext = mockContext.Object
            };

            // Controller should render view
            controller.WithCallTo(x => x.Edit(offerVM, null))
                .ShouldRenderDefaultView()
                .WithModel<EditOfferViewModel>();
        }

        [TestMethod]
        public void Edit_Post_ShouldRedirectIfSuccessful()
        {
            // Arrange
            var offerVM = new EditOfferViewModel { Id = 1, Name = "Test", UserId = user.Id, CategoryName = "Test" };
            var offer = new Offer() { Id = 1, Name = "Test", Owner = user };
            var categories = new List<Category>();
            categories.Add(new Category { Id = 1, Name = "Test" });

            // Setup repositories
            mockedOfferRepo.Setup(r => r.Find(It.IsAny<int>())).Returns(offer);
            mockedCategoriesRepo.Setup(r => r.All()).Returns(categories.AsQueryable());

            // Setup context to have user as not administrator
            Mock<ControllerContext> mockContext = GetContext(true);

            // Setup controller
            var controller = new OffersController(mockedContext.Object, user)
            {
                ControllerContext = mockContext.Object
            };

            // Controller should redirect
            controller.WithCallTo(x => x.Edit(offerVM, null))
                .ShouldRedirectTo("/Offers/Index");
        }

        [TestMethod]
        public void Delete_ShouldRedirectIfNotLogged()
        {
            // Setup controller with no user
            var controller = new OffersController(mockedContext.Object);

            // Controller should redirect
            controller.WithCallTo(x => x.Delete(1))
                .ShouldRedirectTo("/Offers/Index");
        }

        [TestMethod]
        public void Delete_ShouldRedirectIfOfferDoesntExist()
        {
            // Setup repositories
            mockedOfferRepo.Setup(r => r.Find(It.IsAny<int>())).Returns<IQueryable<Offer>>(null);

            // Setup controller
            var controller = new OffersController(mockedContext.Object, user);

            // Controller should redirect
            controller.WithCallTo(x => x.Delete(1))
                .ShouldRedirectTo("/Offers/Index");
        }

        [TestMethod]
        public void Delete_ShouldRedirectIfUserIsNotOwnerOrAdmin()
        {
            // Arrange
            var notOwnerUser = new User() { Id = "2", UserName = "Not owner", PasswordHash = "1234", FullName = "Test" };
            var offer = new Offer() { Id = 1, Name = "Test", Owner = user };

            // Setup repositories
            mockedOfferRepo.Setup(r => r.Find(It.IsAny<int>())).Returns(offer);

            // Setup context to have user as not administrator
            Mock<ControllerContext> mockContext = GetContext(false);

            // Setup controller
            var controller = new OffersController(mockedContext.Object, notOwnerUser)
            {
                ControllerContext = mockContext.Object
            };

            // Controller should redirect
            controller.WithCallTo(x => x.Delete(1))
                .ShouldRedirectTo("/Offers/Index");
        }

        [TestMethod]
        public void Delete_ShouldRedirectIfSuccessfull()
        {
            // Arrange
            var notOwnerUser = new User() { Id = "2", UserName = "Not owner", PasswordHash = "1234", FullName = "Test" };
            var offer = new Offer() { Id = 1, Name = "Test", Owner = user };

            // Setup repositories
            mockedOfferRepo.Setup(r => r.Find(It.IsAny<int>())).Returns(offer);

            // Setup context to have user as administrator
            Mock<ControllerContext> mockContext = GetContext(true);

            // Setup controller
            var controller = new OffersController(mockedContext.Object, notOwnerUser)
            {
                ControllerContext = mockContext.Object
            };

            // Controller should redirect
            controller.WithCallTo(x => x.Delete(1))
                .ShouldRedirectTo("/Offers/Index");
        }

        private static Mock<ControllerContext> GetContext(bool isAdmin)
        {
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.Setup(p => p.IsInRole("Administrator")).Returns(isAdmin);

            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            return mockContext;
        }
    }
}
