namespace Auction.Data
{
    using Migrations;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System.Data.Entity;
    public class AuctionContext : IdentityDbContext<User>
    {
        public AuctionContext()
            : base("name=AuctionContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AuctionContext, Configuration>());
        }

        public IDbSet<Bid> Bids { get; set; }
        public IDbSet<Category> Categories { get; set; }
        public IDbSet<Offer> Offers { get; set; }
        public override IDbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Offer>()
                .HasRequired(x => x.Owner)
                .WithMany(x => x.OwnOffers)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Offer>()
                .HasOptional(x => x.Buyer)
                .WithMany(x => x.BoughtOffers)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }

        public static AuctionContext Create()
        {
            return new AuctionContext();
        }
    }
}