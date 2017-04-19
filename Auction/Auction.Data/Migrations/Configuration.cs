namespace Auction.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Data;
    using Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;
    using System.IO;

    public sealed class Configuration : DbMigrationsConfiguration<Auction.Data.AuctionContext>
    {
        private const string DefaultAdminEmail = "admin@admin.com";
        private const string DefaultAdminFullName = "Admin";
        private const string DefaultAdminPhoneNumber = "07001700";
        private const string DefaultAdminPassword = "123456";
        private const string AdministratorRoleName = "Administrator";

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(AuctionContext context)
        {
            if(!context.Users.Any(x => x.Email == DefaultAdminEmail))
            {
                var adminEmail = DefaultAdminEmail;
                var adminUsername = adminEmail;
                var adminFullName = DefaultAdminFullName;
                var adminPhoneNumber = DefaultAdminPhoneNumber;
                var adminPassword = DefaultAdminPassword;
                var adminRole = AdministratorRoleName;

                CreateAdminUser(context, adminEmail, adminUsername, adminFullName, adminPhoneNumber, adminPassword, adminRole);
            }

            if (!context.Categories.Any())
            {
                AddCategories(context);
            }
        }

        private void CreateAdminUser(AuctionContext context, string adminEmail, string adminUsername, string adminFullName, string adminPhoneNumber, string adminPassword, string adminRole)
        {
            //Create admin role
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            roleManager.Create(new IdentityRole(AdministratorRoleName));

            var adminUser = new User
            {
                UserName = adminUsername,
                Email = adminEmail,
                FullName = adminFullName,
                PhoneNumber = adminPhoneNumber
            };

            var userStore = new UserStore<User>(context);
            var userManager = new UserManager<User>(userStore);

            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };

            var userCreateResult = userManager.Create(adminUser, adminPassword);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }

            var addAdminRoleResult = userManager.AddToRole(adminUser.Id, adminRole);
            if (!addAdminRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addAdminRoleResult.Errors));
            }

            context.SaveChanges();
        }

        private void AddCategories(AuctionContext context)
        {
            context.Categories.Add(new Category { Name = "Home" });
            context.Categories.Add(new Category { Name = "Office" });
            context.Categories.Add(new Category { Name = "Clothes" });
            context.Categories.Add(new Category { Name = "Technology" });

            context.SaveChanges();
        }
    }
}
