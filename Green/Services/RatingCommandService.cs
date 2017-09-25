using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Green.Entities;
using Green.Models;
using Green.Services;

namespace Green.Services
{
    public class RatingCommandService : IRatingCommandService
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();

        private const string SuccessMessage = "Action sucessfully performed.";
        private const string ErrorMessage = "An application exception occured performing action.";
        private const string ItemNotFoundMessage = "The item was not found.";

        public string SaveRating(Rating rating)
        {
            try
            {
                var oldRating = ctx.Ratings.FirstOrDefault(r => r.ClientId == rating.ClientId && r.RestaurantId == rating.RestaurantId);
                if (oldRating == null)
                {
                    rating.Id = Guid.NewGuid().ToString();
                    ctx.Ratings.Add(rating);
                }
                else
                {
                    oldRating.Value = rating.Value;
                }

                ctx.SaveChanges();
                return SuccessMessage;
            }
            catch (Exception)
            {
                return ErrorMessage;
            }
        }
        public string DeleteRating(string id)
        {
            try
            {
                return SuccessMessage;
            }
            catch (Exception)
            {
                return ErrorMessage;
            }
        }

        public string DeleteRatingsForRestaurant(string restaurantId)
        {
            try
            {
                var ratings = ctx.Ratings.Where(r => r.RestaurantId == restaurantId).ToList();
                if (ratings.Any())
                {
                    ctx.Ratings.RemoveRange(ratings);
                    ctx.SaveChanges();
                }
                return SuccessMessage;
            }
            catch (Exception)
            {
                return ErrorMessage;
            }
        }
    }
}