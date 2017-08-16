using Green.Entities;
using Green.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Green.Services
{
    public class RestaurantCommandService
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();
        private const string SuccessMessage = "Action sucessfully performed.";
        private const string ErrorMessage = "An application exception occured performing action.";
        private const string ItemNotFoundMessage = "The item was not found.";
        public string SaveRestaurant(Restaurant restaurant)
        {
            try
            {
                var oldRestaurant = ctx.Restaurants.FirstOrDefault(f => f.id == restaurant.id);
                if (oldRestaurant == null)
                {
                    restaurant.id = Guid.NewGuid().ToString();
                    ctx.Restaurants.Add(restaurant);
                }
                else
                {
                    oldRestaurant.Name = restaurant.Name;
                    oldRestaurant.Type = restaurant.Type;
                    oldRestaurant.Address = restaurant.Address;
                    oldRestaurant.MaxPrice = restaurant.MaxPrice;
    
                }

                ctx.SaveChanges();
                return SuccessMessage;
            }
            catch
            {
                return ErrorMessage;
            }
        }

        public string DeleteRestaurant(string id)
        {
            try
            {
                var restaurant = ctx.Restaurants.FirstOrDefault(f => f.id == id);
                if (restaurant!= null)
                {
                    ctx.Restaurants.Remove(restaurant);
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