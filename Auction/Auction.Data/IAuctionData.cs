using Auction.Data.Repositories;
using Auction.Models;

namespace Auction.Data
{
    public interface IAuctionData
    {
        IRepository<Bid> Bids { get; }

        IRepository<Category> Categories { get; }

        IRepository<Offer> Offers { get; }

        IRepository<User> Users { get; }

        int SaveChanges();
    }
}
