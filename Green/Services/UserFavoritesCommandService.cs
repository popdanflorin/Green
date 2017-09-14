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
        private const string SuccessMessage = "The restaurant was added to favorites list!";
        private const string ErrorMessage = "An application exception occured performing action.";
        private const string ItemNotFoundMessage = "The item was not found.";
        private const string EmptyInputMessage = "The inputs are empty";
        private const string AlertMessage = "The restaurant is already in your favorites list!";
        public string SaveFavorite(UserFavorites userFavorite)
        {
            try
            {
             
                var oldFavorite = ctx.UserFavorites.FirstOrDefault(r => r.ClientId == userFavorite.ClientId && r.RestaurantId==userFavorite.RestaurantId);
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
        public string DeleteFavorite(string Id)
        {
            try
            {
                var favorite = ctx.UserFavorites.FirstOrDefault(f => f.RestaurantId == Id);
                if (favorite != null)
                {
                    ctx.UserFavorites.Remove(favorite);
                    ctx.SaveChanges();
                    return SuccessMessage;
                }
                return ItemNotFoundMessage;
            }
            catch (Exception)
            {
                return ErrorMessage;
            }
        }
    }
}