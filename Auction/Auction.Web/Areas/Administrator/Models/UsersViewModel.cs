using Auction.Models;
using System;
using System.Linq.Expressions;

namespace Auction.Web.Areas.Administrator.Models
{
    public class UsersViewModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public static Expression<Func<User, UsersViewModel>> Create
        {
            get
            {
                return u => new UsersViewModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Username = u.UserName
                };
            }
        }
    }
}