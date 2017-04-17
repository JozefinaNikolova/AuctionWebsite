using Auction.Models;
using System;
using System.Linq.Expressions;

namespace Auction.Web.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static Expression<Func<Category, CategoryViewModel>> Create
        {
            get
            {
                return x => new CategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                };
            }
        }
    }
}