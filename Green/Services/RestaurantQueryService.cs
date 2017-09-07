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
        public List<string> GetRestaurantTypesString()
        {
            List<string> StringTypes = new List<string>();
            List<EnumItem> Types = Enum.GetValues(typeof(RestaurantType)).Cast<RestaurantType>().Select(x => new EnumItem() { Id = (int)x, Description = x.ToString() }).ToList();
            foreach (var item in Types)
            {
                StringTypes.Add(item.Description);
            }
            return StringTypes;
        }
        public List<Rating> GetRatings()
        {
            return ctx.Ratings.ToList();
        }
        public List<Image> GetRestaurantImages(string restaurantId)
        {
            // get cover image first
            var images = ctx.Images.Where(i => i.RestaurantId == restaurantId);
            if (!images.Any())
                return null;
            return images.OrderByDescending(i => i.isCover).ToList();
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
        public Image GetRestaurantCover(string restaurantId)
        {
            var images = ctx.Images.Where(i => i.RestaurantId == restaurantId);
            if (!images.Any())
                return null;
            var cover = images.FirstOrDefault(i => i.isCover);
            if (cover != null)
                return cover;
            return images.First();
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
                //   var rating = listRatings.FirstOrDefault(x => x.RestaurantId == item.id);                
                var image = listImages.FirstOrDefault(x => x.RestaurantId == item.id);
                if (image != null)
                    userRestaurant.ImageName = image.Name;
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