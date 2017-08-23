using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using Green.Services;
using Green.Entities;
using Microsoft.AspNet.Identity;

namespace Green.Controllers
{
    public class ReservationsController : Controller
    {
        private ReservationQueryService qReservationService = new ReservationQueryService();
        private ReservationCommandService cReservationService = new ReservationCommandService();

        private RestaurantQueryService qRestaurantService = new RestaurantQueryService();
        private RestaurantCommandService cRestaurantService = new RestaurantCommandService();

        // GET: Reservations
        [Authorize]
        public ActionResult List()
        {
            return View();
        }

        public JsonResult ListRefresh()
        {
            var reservations = qReservationService.GetReservations();
            var restaurants = qRestaurantService.GetRestaurants();
            var isAdmin = User.IsInRole("AppAdmin");
            var userId = User.Identity.GetUserId();
            if (!isAdmin)
                reservations = reservations.Where(r => r.ClientId == userId).ToList();
            return new JsonResult { Data = new { Reservations = reservations, Restaurants = restaurants, isAdmin = isAdmin, UserId = userId }, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        //public JsonResult RestaurantInfo(string restaurantId)
        //{
            
        //}

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
