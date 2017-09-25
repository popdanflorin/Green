using Green.Entities;
using Green.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Green.Interfaces;

namespace Green.Services
{
    public class UserFavoritesQueryService : IUserFavoritesQueryService
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();

        public List<UserFavorites> GetUsersFavorites()
        {
            return ctx.UserFavorites.ToList();
        }
        public List<Restaurant> GetRestaurants()
        {
            return ctx.Restaurants.ToList();
        }
        public List<Image> GetImages()
        {
            return ctx.Images.ToList();
        }
       
        public List<UserRestaurant> GetUserFavorites(string UserId)
        {
            List<Restaurant> listRestaurants = GetRestaurants();
            List<UserFavorites> totalUserFavorites = GetUsersFavorites();
            List<Restaurant> listUserFavorites = new List<Restaurant>();
            List<Image> listImages = GetImages();
            List<UserRestaurant> listUserRestaurants = new List<UserRestaurant>();
            var  newListUserFavorites= totalUserFavorites.Where(x=>x.ClientId==UserId);
            foreach(var item in newListUserFavorites)
            {
                listUserFavorites.Add(listRestaurants.FirstOrDefault(x=>x.id==item.RestaurantId));
            }
            foreach(var item in listUserFavorites)
            {
                var userRestaurant = new UserRestaurant();
                userRestaurant.id = item.id;
                userRestaurant.Name = item.Name;
                userRestaurant.Address = item.Address+", "+item.City;
                userRestaurant.Type = item.Type;
                var image = listImages.FirstOrDefault(x => x.RestaurantId == item.id);
                if (image != null)
                    userRestaurant.ImageName = image.Name;
                else
                    userRestaurant.ImageName = "noimage.jpg";
                listUserRestaurants.Add(userRestaurant);
            }
            return listUserRestaurants;
        }
    }
}