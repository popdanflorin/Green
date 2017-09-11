using Green.Entities;
using Green.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Green.Services
{
    public class UserFavoritesCommandService
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();
        private const string SuccessMessage = "Action sucessfully performed.";
        private const string ErrorMessage = "An application exception occured performing action.";
        private const string ItemNotFoundMessage = "The item was not found.";
        private const string EmptyInputMessage = "The inputs are empty";
        private const string AlertMessage = "The restaurant is already in your favorites list";
        public string SaveFavorite(UserFavorites userFavorite)
        {
            try
            {
             
                var oldFavorite = ctx.Ratings.FirstOrDefault(r => r.ClientId == userFavorite.ClientId && r.RestaurantId==userFavorite.RestaurantId);
                if (oldFavorite == null)
                {
                    userFavorite.Id = Guid.NewGuid().ToString();
                    ctx.UserFavorites.Add(userFavorite);
                    ctx.SaveChanges();
                    return SuccessMessage;
                }
                else
                    return AlertMessage;
               
            }
            catch (Exception)
            {
                return ErrorMessage;
            }
        }
    }
}