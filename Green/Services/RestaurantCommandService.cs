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
        private const string EmptyInputMessage = "The inputs are empty";
        /* public string UploadImage(Image image)
         {
             try
             {
                 var oldImage = ctx.Images.FirstOrDefault(f=>f.Id==image.Id);
                 if (oldImage == null)
                 {
                     image.Id = Guid.NewGuid().ToString();
                     ctx.Images.Add(image);
                 }
                 else
                 {
                     oldImage.Name = image.Name;
                     oldImage.Data = image.Data;
                     oldImage.MealId = image.MealId;
                     oldImage.RestaurantId = image.RestaurantId;
                 }
                 ctx.SaveChanges();
                 return SuccessMessage;
             }
             catch
             {
                 return ErrorMessage;
             }
         }*/

        public string SaveRestaurant(Restaurant restaurant)
        {
            try
            {
                if (restaurant.Name == null || restaurant.Address == null )
                {
                    return EmptyInputMessage;
                }

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
                    oldRestaurant.SeatsAvailable = restaurant.SeatsAvailable;
                    oldRestaurant.OpeningHour = restaurant.OpeningHour;
                    oldRestaurant.ClosingHour = restaurant.ClosingHour;

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
                if (restaurant != null)
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