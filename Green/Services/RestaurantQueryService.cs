﻿using System;
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
    }
}