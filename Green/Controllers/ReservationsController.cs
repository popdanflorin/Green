using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using Green.Services;
using Green.Entities;
using Green.Interfaces;
using Microsoft.AspNet.Identity;

namespace Green.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private IReservationQueryService qReservationService;
        private IReservationCommandService cReservationService;

        private IRestaurantQueryService qRestaurantService;
        private IRestaurantCommandService cRestaurantService;

        public ReservationsController(IReservationCommandService _cReservationService, IReservationQueryService _qReservationService,
            IRestaurantCommandService _cRestaurantService, IRestaurantQueryService _qRestaurantService)
        {
            cReservationService = _cReservationService;
            qReservationService = _qReservationService;
            cRestaurantService = _cRestaurantService;
            qRestaurantService = _qRestaurantService;
        }

        public ActionResult UserReservations()
        {
            return View();
        }

        [Authorize(Roles = "AppAdmin")]
        public ActionResult List()
        {
            return View();
        }

        public JsonResult ListRefresh()
        {
            var reservations = qReservationService.GetReservations().Where(r => r.Restaurant.OwnerId == User.Identity.GetUserId()).ToList();
            var restaurants = qRestaurantService.GetRestaurants().Where(r => r.OwnerId == User.Identity.GetUserId()).ToList();
            var userId = User.Identity.GetUserId();
            return new JsonResult { Data = new { Reservations = reservations, Restaurants = restaurants, UserId = userId }, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult ListRefreshUser()
        {
            var reservations = qReservationService.GetReservations().Where(r => r.ClientId == User.Identity.GetUserId()).ToList();
            var restaurants = qRestaurantService.GetRestaurants().ToList();
            var userId = User.Identity.GetUserId();
            return new JsonResult { Data = new { Reservations = reservations, Restaurants = restaurants, UserId = userId }, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult RefreshReservations(string restaurantId, int year, int month)
        {
            var reservations = qReservationService.GetReservations(restaurantId, year, month);
            return new JsonResult { Data = new { Reservations = reservations }, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult ReservationsPercentageRefresh(int year)
        {
            var restaurants = qRestaurantService.GetRestaurants().Where(r => r.OwnerId.CompareTo(User.Identity.GetUserId()) == 0).ToList();
            var reservationsPercentage = qReservationService.GetSeatsPercetageForAllPerYear(restaurants, year);
            return new JsonResult { Data = reservationsPercentage, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult RestaurantPercentageRefresh(string restaurantId, int year, int month)
        {
            var restaurantPercentage = qReservationService.GetSeatsPercentageForRestaurantPerMonth(restaurantId, year, month);
            return new JsonResult { Data = restaurantPercentage, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult RestaurantInfo(string restaurantId)
        {
            var restaurant = qRestaurantService.GetRestaurants().FirstOrDefault(r => r.id == restaurantId);
            var openingHour = -1;
            var closingHour = -1;
            if (restaurant != null)
            {
                openingHour = restaurant.OpeningHour;
                closingHour = restaurant.ClosingHour;
            }

            return new JsonResult { Data = new { OpeningHour = openingHour, ClosingHour = closingHour }, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult Save(Reservation reservation)
        {
            var message = cReservationService.SaveReservation(reservation);
            return new JsonResult() { Data = message, ContentEncoding = Encoding.UTF8 };
        }

        [HttpPost]
        public JsonResult Delete(string reservationId)
        {
            var message = cReservationService.DeleteReservation(reservationId);
            return new JsonResult() { Data = message, ContentEncoding = Encoding.UTF8 };
        }
    }
}
