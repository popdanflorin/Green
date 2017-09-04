using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Green.Entities;
using Green.Entities.Enums;
using Green.Models;

namespace Green.Services
{
    public class RestaurantQueryService
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();

        public List<Restaurant> GetRestaurants()
        {
            return ctx.Restaurants.ToList();
        }
        public List<EnumItem> GetRestaurantTypes()  //transform the enum into a list of restaurants types
        {
            return Enum.GetValues(typeof(RestaurantType)).Cast<RestaurantType>().Select(x => new EnumItem() { Id = (int)x, Description = x.ToString() }).ToList();
        }
        public List<Image> GetImages()
        {
            return ctx.Images.ToList();
        }
        public List<Rating> GetRatings()
        {
            return ctx.Ratings.ToList();
        }
        public List<string> GetRestaurantImages(string restaurantId)
        {
            var images = ctx.Images.Where(i => i.RestaurantId == restaurantId);
            if (images.Any())
                return images.Select(i => i.Name).ToList();
            return null;
        }
        public List<UserRestaurant> GetUserRestaurants()
        {
            List<Restaurant> listRestaurants = GetRestaurants();
            List<Image> listImages = GetImages();
            List<UserRestaurant> listUserRestaurants = new List<UserRestaurant>();
            List<Rating> listRatings = GetRatings();
            foreach (var item in listRestaurants)
            {
                var userRestaurant = new UserRestaurant();
                userRestaurant.id = item.id;
                userRestaurant.Name = item.Name;
                userRestaurant.Address = item.Address;
                userRestaurant.Type = item.Type;
                var rating = listRatings.FirstOrDefault(x => x.RestaurantId == item.id);
                var image = listImages.FirstOrDefault(x => x.RestaurantId == item.id);
                if (image != null)
                    userRestaurant.ImageName = image.Name;
                if (rating != null)
                    userRestaurant.Rating = rating.Value;
                listUserRestaurants.Add(userRestaurant);

            }
            return listUserRestaurants;
        }
        public List<UserRestaurant> GetUserRestaurants(string name)
        {
            List<Restaurant> listRestaurants = GetRestaurants();
            List<Image> listImages = GetImages();
            List<UserRestaurant> listUserRestaurants = new List<UserRestaurant>();
            var listNew = listRestaurants.Where(x => x.Name == name);
            
            foreach (var item in listNew)
            {
                var userRestaurant = new UserRestaurant();
                userRestaurant.id = item.id;
                userRestaurant.Name = item.Name;
                userRestaurant.Address = item.Address;
                userRestaurant.Type = item.Type;
                
                var image = listImages.FirstOrDefault(x => x.RestaurantId == item.id);
                if (image != null)
                    userRestaurant.ImageName = image.Name;
               
                listUserRestaurants.Add(userRestaurant);

            }
            return listUserRestaurants;

        }
    }
}