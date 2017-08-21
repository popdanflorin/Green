﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Green.Entities;
using Green.Entities.Enums;
using Green.Services;
using System.Text;

namespace Green.Controllers
{
    public class RestaurantsController : Controller
    {
        private RestaurantQueryService qService = new RestaurantQueryService();
        private RestaurantCommandService cService = new RestaurantCommandService();
        // GET: Restaurants

        [Authorize (Roles = "AppAdmin")]
        public ActionResult List()
        {
            return View();
        }
        public ActionResult UserRestaurants()
        {
            return View();
        }
        public JsonResult ListRefresh()
        {
            var restaurants = qService.GetRestaurants();
            var restaurantTypes = qService.GetRestaurantTypes();
            return new JsonResult() { Data = new { Restaurants = restaurants, RestaurantTypes = restaurantTypes }, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult Save(Restaurant restaurant)
        {
            var message = cService.SaveRestaurant(restaurant);
            return new JsonResult() { Data = message, ContentEncoding = Encoding.UTF8 };
        }
        public JsonResult Delete(string restaurantId)
        {
            var message = cService.DeleteRestaurant(restaurantId);
            return new JsonResult() {Data=message,ContentEncoding=Encoding.UTF8};
        }
    }
}