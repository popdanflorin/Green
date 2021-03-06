﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Green.Entities;
using Green.Entities.Enums;
using Green.Services;
using System.Text;
using Green.Models;
using System.IO;
using Microsoft.AspNet.Identity;
using Green.Interfaces;

namespace Green.Controllers
{
    public class RestaurantsController : Controller
    {
        private const string SuccessMessage = "Action sucessfully performed.";
        private const string ErrorMessage = "An application exception occured performing action.";

        private IRestaurantQueryService qService;
        private IRestaurantCommandService cService;

        private IMenuQueryService qMenuService;
        private IMenuCommandService cMenuService;

        public RestaurantsController(IRestaurantCommandService _cService, IRestaurantQueryService _qService,
            IMenuCommandService _cMenuService, IMenuQueryService _qMenuService)
        {
            cService = _cService;
            qService = _qService;
            cMenuService = _cMenuService;
            qMenuService = _qMenuService;
        }

        // GET: Restaurants

        [Authorize(Roles = "AppAdmin")]
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
            var restaurants = qService.GetRestaurants().Where(r => r.OwnerId == User.Identity.GetUserId()).ToList();
            var restaurantTypes = qService.GetRestaurantTypes();
            var images = qService.GetImages();
            return new JsonResult() { Data = new { Restaurants = restaurants, RestaurantTypes = restaurantTypes, Images = images }, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetCurrentMenu(string restaurantId)
        {
            var menu = qMenuService.GetCurrentMenu(restaurantId);
            return new JsonResult() { Data = menu, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult Save(Restaurant restaurant)
        {
            restaurant.OwnerId = User.Identity.GetUserId();
            var message = cService.SaveRestaurant(restaurant);
            return new JsonResult() { Data = message, ContentEncoding = Encoding.UTF8 };
        }
        public JsonResult Delete(string restaurantId)
        {
            // delete images from database and folder (if exists)
            string physicalPath;
            var images = cService.DeleteImages(restaurantId);
            if (images.Any())
                images.ForEach(i =>
                {
                    physicalPath = Server.MapPath("~/Content/images/" + i);
                    System.IO.File.Delete(physicalPath);
                });

            var message = cService.DeleteRestaurant(restaurantId);
            return new JsonResult() { Data = message, ContentEncoding = Encoding.UTF8 };
        }
        public JsonResult DeleteImage(string imageId)
        {
            var imageName = cService.DeleteImage(imageId);
            string message = ErrorMessage;
            if (imageName != null)
            {
                message = SuccessMessage;
                System.IO.File.Delete(Server.MapPath("~/Content/images/" + imageName));
            }
            return new JsonResult() { Data = message, ContentEncoding = Encoding.UTF8 };
        }
        public JsonResult SetCoverImage(string restaurantId, string imageId)
        {
            var message = cService.SetCoverImage(restaurantId, imageId);
            return new JsonResult() { Data = message, ContentEncoding = Encoding.UTF8 };
        }
        public JsonResult GetRestaurantImages(string restaurantId)
        {
            var images = qService.GetRestaurantImages(restaurantId);
            return new JsonResult() { Data = new { Images = images }, ContentEncoding = Encoding.UTF8 };
        }
        public JsonResult UserRestaurantsRefresh()
        {
            var userRestaurants = qService.GetUserRestaurants();
            var types = qService.GetRestaurantTypesString();
            var userId = User.Identity.GetUserId();
            return new JsonResult() { Data = new { UserRestaurants = userRestaurants, Types = types, UserId = userId }, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        [HttpPost]
        public JsonResult GetRatings(string restaurantId)
        {
            var totalRating = qService.GetTotalRating(restaurantId);
            return new JsonResult() { Data = new { TotalRating = totalRating }, ContentEncoding = Encoding.UTF8 };
        }
        public JsonResult UserRestaurantsSearch(string restaurantName,string mealName,RestaurantType restaurantType,string cityName)
        {
            var userRestaurants = qService.GetUserRestaurantsByAll(restaurantName,mealName,restaurantType,cityName);
            return new JsonResult() { Data = new { UserRestaurants = userRestaurants }, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult UserRestaurantsSearchByName(string restaurantName)
        {
            var userRestaurants = qService.GetUserRestaurants(restaurantName);
            return new JsonResult() { Data = new { UserRestaurants = userRestaurants }, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult UserRestaurantsSearchByType(RestaurantType restaurantType)
        {
            var userRestaurants = qService.GetUserRestaurants(restaurantType);
            return new JsonResult() { Data = new { UserRestaurants = userRestaurants }, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        //upload images
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file, string rid)
        {
            if (file != null && rid != null && rid.Length != 0)
            {
                ApplicationDbContext db = new ApplicationDbContext();
                string ImageName = System.IO.Path.GetFileName(file.FileName);
                string physicalPath = Server.MapPath("~/Content/images/" + ImageName);

                // save image in folder
                file.SaveAs(physicalPath);

                //save new record in database
                Image newRecord = new Image();
                newRecord.Id = Guid.NewGuid().ToString();
                newRecord.Name = ImageName;

                newRecord.RestaurantId = rid;
                db.Images.Add(newRecord);
                db.SaveChanges();

            }
            //Display records
            return RedirectToAction("List");
        }

    }

}