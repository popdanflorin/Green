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
        
        public int GetUserRating(string RestaurantId)
        {

            return 0;
        }

    }
}