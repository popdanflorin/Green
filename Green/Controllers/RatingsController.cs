using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Green.Services;
using Green.Entities;
using Microsoft.AspNet.Identity;
using System.Text;

namespace Green.Controllers
{
    public class RatingsController : Controller
    {
        private ReservationQueryService qReservationService = new ReservationQueryService();
        private ReservationCommandService cReservationService = new ReservationCommandService();

        private RestaurantQueryService qRestaurantService = new RestaurantQueryService();
        private RestaurantCommandService cRestaurantService = new RestaurantCommandService();

        private RatingQueryService qRatingService = new RatingQueryService();
        private RatingCommandService cRatingService = new RatingCommandService();
        
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
            var value = qRatingService.GetUserRating(RestaurantId);
            return new JsonResult() { Data = value, ContentEncoding = Encoding.UTF8 };
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
