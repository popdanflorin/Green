using Green.Entities;
using Green.Entities.Enums;
using Green.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Green.Interfaces
{
    public interface IRestaurantQueryService
    {
        List<Restaurant> GetRestaurants();
        List<EnumItem> GetRestaurantTypes();
        List<Image> GetImages();
        List<string> GetRestaurantTypesString();
        List<Rating> GetRatings();
        List<Meal> GetMeals();
        List<MenuMeal> GetMenuMeals();
        List<Menu> GetMenus();
        List<Image> GetRestaurantImages(string restaurantId);
        int GetTotalRating(string RestaurantId);
        Image GetRestaurantCover(string restaurantId);
        List<UserRestaurant> GetUserRestaurants();
        List<UserRestaurant> GetUserRestaurants(string name);
        List<UserRestaurant> GetUserRestaurantsByAll(string restaurantName, string mealName, RestaurantType type, string cityName);
        List<UserRestaurant> GetUserRestaurants(RestaurantType type);
    }
}
