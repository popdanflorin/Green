using Green.Entities;
using Green.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Green.Services
{
    public class RatingQueryService : IRatingQueryService
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();

        public List<Rating> GetRatings()
        {
            var ratings = ctx.Ratings;
            return ratings.ToList();
        }

        public int GetUserRating(string UserId, string RestaurantId)
        {
            var rating = ctx.Ratings.FirstOrDefault(r => r.ClientId == UserId && r.RestaurantId == RestaurantId);
            if (rating != null)
                return rating.Value;
            return 0;
        }
        public int GetTotalRating(string RestaurantId)
        {
            var ratings = ctx.Ratings.Where(r => r.RestaurantId == RestaurantId);
            if (ratings.Any())
            {
                return ratings.Sum(r => r.Value) / ratings.Count();
            }
            return 0;
        }

    }
}