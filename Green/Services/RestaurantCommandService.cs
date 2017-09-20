﻿using Green.Entities;
using Green.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Green.Services
{
    public class RestaurantCommandService
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();
        private const string SuccessMessage = "Action sucessfully performed.";
        private const string ErrorMessage = "An application exception occured performing action.";
        private const string ItemNotFoundMessage = "The item was not found.";
        private const string EmptyInputMessage = "The inputs are empty";

        private MenuQueryService qMenuService = new MenuQueryService();
        private MenuCommandService cMenuService = new MenuCommandService();

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
                if (restaurant.Name == null || restaurant.Address == null)
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
                    oldRestaurant.OwnerId = restaurant.OwnerId;
                    oldRestaurant.Name = restaurant.Name;
                    oldRestaurant.Type = restaurant.Type;
                    oldRestaurant.Address = restaurant.Address;
                    oldRestaurant.City = restaurant.City;
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

        public string SetCoverImage(string restaurantId, string imageId)
        {
            try
            {
                var images = ctx.Images;
                foreach (var i in images)
                {
                    if (i.Id == imageId)
                        i.isCover = true;
                    else
                        i.isCover = false;
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
                    // delete menu(s)
                    var message = cMenuService.DeleteRestaurantMenus(id);

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
        
        public List<String> DeleteImages(string restaurantId)
        {
            var images = ctx.Images.Where(i => i.RestaurantId == restaurantId).ToList();
            if (images.Any())
            {
                var imagesName = images.Select(i => i.Name).ToList();
                return imagesName;
            }
            return null;
        }

        public String DeleteImage(string imageId)
        {
            try
            {
                var image = ctx.Images.FirstOrDefault(i => i.Id == imageId);
                var imageName = image.Name;
                if (image != null)
                {
                    ctx.Images.Remove(image);
                    ctx.SaveChanges();
                    return imageName;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}