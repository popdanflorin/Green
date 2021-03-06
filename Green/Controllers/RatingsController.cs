﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Green.Services;
using Green.Entities;
using Microsoft.AspNet.Identity;
using System.Text;
using Green.Interfaces;

namespace Green.Controllers
{
    public class RatingsController : Controller
    {
        private IReservationQueryService qReservationService;
        private IReservationCommandService cReservationService;

        private IRestaurantQueryService qRestaurantService;
        private IRestaurantCommandService cRestaurantService;

        private IRatingQueryService qRatingService;
        private IRatingCommandService cRatingService;
        
        public RatingsController(IRatingCommandService _cRatingService, IRatingQueryService _qRatingService, IReservationCommandService _cReservationService, IReservationQueryService _qReservationService,
            IRestaurantCommandService _cRestaurantService, IRestaurantQueryService _qRestaurantService)
        {
            cRatingService = _cRatingService;
            qRatingService = _qRatingService;
            cReservationService = _cReservationService;
            qReservationService = _qReservationService;
            cRestaurantService = _cRestaurantService;
            qRestaurantService = _qRestaurantService;
        }

        // GET: Ratings
        public ActionResult List()
        {
            return View();
        }
        public JsonResult GetUserId()
        {
            var userId = User.Identity.GetUserId();
            return new JsonResult { Data = new { UserId = userId }, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetUserRating(string RestaurantId)
        {
            var userId = User.Identity.GetUserId();
            var value = qRatingService.GetUserRating(userId, RestaurantId);
            return new JsonResult() { Data = value, ContentEncoding = Encoding.UTF8 };
        }

        [HttpPost]
        public JsonResult GetRatings(Rating rating)
        {
            var restaurantId = rating.RestaurantId;
            var userId = User.Identity.GetUserId();
            var userRating = qRatingService.GetUserRating(userId, restaurantId);
            var totalRating = qRatingService.GetTotalRating(restaurantId);
            return new JsonResult() { Data = new { UserRating = userRating, TotalRating = totalRating }, ContentEncoding = Encoding.UTF8 };
        }

        public JsonResult Refresh()
        {
            var userId = User.Identity.GetUserId();
            var restaurants = qRestaurantService.GetRestaurants();
            var ratings = qRatingService.GetRatings();
            var userRatings = ratings.Where(r => r.ClientId == userId);

            return new JsonResult { Data = new { UserId = userId }, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult Save(Rating rating)
        {
            var message = cRatingService.SaveRating(rating);
            return new JsonResult() { Data = message, ContentEncoding = Encoding.UTF8 };
        }

    }
}
