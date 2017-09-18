using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Green.Entities;
using Green.Entities.Enums;
using Green.Models;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using Green.Services;

using System.Text;

namespace Green.Services
{
    public class RestaurantQueryService
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();

        public List<Restaurant> GetRestaurants()
        {
             return ctx.Restaurants.OrderBy(r => r.Name).ToList();
        }
        public List<EnumItem> GetRestaurantTypes()  //transform the enum into a list of restaurants types
        {
            return Enum.GetValues(typeof(RestaurantType)).Cast<RestaurantType>().Select(x => new EnumItem() { Id = (int)x, Description = EnumExtensions.ToString(x) }).ToList();
        }
        public List<Image> GetImages()
        {
            return ctx.Images.ToList();
        }
        public List<string> GetRestaurantTypesString()
        {
            List<string> StringTypes = new List<string>();
            List<EnumItem> Types = Enum.GetValues(typeof(RestaurantType)).Cast<RestaurantType>().Select(x => new EnumItem() { Id = (int)x, Description = EnumExtensions.ToString(x) }).ToList();
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
        public List<Meal> GetMeals()
        {
            return ctx.Meals.ToList();
        }
        public List<MenuMeal> GetMenuMeals()
        {
            return ctx.MenuMeals.ToList();
        }
        public List<Menu> GetMenus()
        {
            return ctx.Menus.ToList();
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
                var ratings = ctx.Ratings.Where(r => r.RestaurantId == item.id);
                if (ratings.Any())
                    userRestaurant.Rating = ratings.Sum(r => r.Value) / ratings.Count();
                else
                    userRestaurant.Rating = 0;
                var image = listImages.FirstOrDefault(x => x.RestaurantId == item.id);
                if (image != null)
                    userRestaurant.ImageName = image.Name;
                else
                    userRestaurant.ImageName = "noimage.jpg";
                listUserRestaurants.Add(userRestaurant);

            }
            return listUserRestaurants;
        }
        public List<UserRestaurant> GetUserRestaurants(string name)
        {
            List<Restaurant> listRestaurants = GetRestaurants();
            List<Image> listImages = GetImages();
            List<UserRestaurant> listUserRestaurants = new List<UserRestaurant>();
            var listNew = listRestaurants.Where(x => x.Name.ToLower().StartsWith(name.ToLower()));
          
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
                else
                    userRestaurant.ImageName = "noimage.jpg";
                listUserRestaurants.Add(userRestaurant);

            }
            return listUserRestaurants;

        }
      /*  public List<UserRestaurant> GetUserRestaurantsByMeal(string menuName)
        {
            List<Restaurant> listRestaurants = GetRestaurants();
            List<Image> listImages = GetImages();
            List<Menu> listMenus = GetMenus();
            List<MenuMeal> listMenuMeals = GetMenuMeals();
            List<Meal> listMeals = GetMeals();
            var listNewMeals = listMeals.Where(x=>x.Name.Contains(menuName));
            List<UserRestaurant> listUserRestaurants = new List<UserRestaurant>();
            List<MenuMeal> listNewMenuMeals = new List<MenuMeal>();
            List<Menu> listNewMenus = new List<Menu>();
            List<Restaurant> listNewRestaurants = new List<Restaurant>();
            foreach (var item in listNewMeals)
            {
                var list = listMenuMeals.Where(x => x.MealId == item.Id);
                listNewMenuMeals.AddRange(list);
            }
            foreach(var item in listNewMenuMeals)
            {
                var list = listMenus.Where(x => x.Id == item.MenuId);
                listNewMenus.AddRange(list);
            }
            foreach(var item in listNewMenus)
            {
                var list = listRestaurants.Where(x => x.id == item.RestaurantId);
                listNewRestaurants.AddRange(list);
            }
            foreach (var item in listNewRestaurants)
            {
                var userRestaurant = new UserRestaurant();
                userRestaurant.id = item.id;
                userRestaurant.Name = item.Name;
                userRestaurant.Address = item.Address;
                userRestaurant.Type = item.Type;
                var image = listImages.FirstOrDefault(x => x.RestaurantId == item.id);
                if (image != null)
                    userRestaurant.ImageName = image.Name;
                else
                    userRestaurant.ImageName = "noimage.jpg";
                listUserRestaurants.Add(userRestaurant);

            }
            return listUserRestaurants;
            var all=listMeals.Join(listMenuMeals,x=>x.Id,y=>y.MealId, (x,y)=>new {x,y}) 
                .Where
                .Join(listMenus,z=>z.MenuId,w=>w.Id,(z,w)=>new {z,w})                
               

        }*/
        public List<UserRestaurant> GetUserRestaurants(RestaurantType type)
        {
            List<Restaurant> listRestaurants = GetRestaurants();
            List<Image> listImages = GetImages();
            List<UserRestaurant> listUserRestaurants = new List<UserRestaurant>();
            var listNew = listRestaurants.Where(x => x.Type == type);
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
                else
                    userRestaurant.ImageName = "noimage.jpg";
                listUserRestaurants.Add(userRestaurant);

            }
            return listUserRestaurants;

        }
        public List<UserRestaurant> SearchUserRestaurants(string restaurantName,string menuName,RestaurantType restaurantType)
        {
            List<UserRestaurant> finalUserRestaurants = new List<UserRestaurant>();
            if (restaurantName == null && menuName == null)
                finalUserRestaurants = GetUserRestaurants();
            
            return finalUserRestaurants;
        }

    }
}