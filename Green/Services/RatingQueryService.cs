using Green.Entities;
using Green.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Green.Services
{
    public class RatingQueryService
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();

        public List<Rating> GetRatings()
        {
            var ratings = ctx.Ratings;
            return ratings.ToList();
        }

        public int GetUserRating(string UserId, string RestaurantId)
        {
            return ctx.Ratings.FirstOrDefault(r => r.ClientId == UserId && r.RestaurantId == RestaurantId).Value;
        }
        public int GetTotalRating(string RestaurantId)
        {
            return ctx.Ratings.Where(r => r.RestaurantId == RestaurantId).Sum(r => r.Value);
        }

    }
}