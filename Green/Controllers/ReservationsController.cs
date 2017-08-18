using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using Green.Services;
using Green.Entities;

namespace Green.Controllers
{
    public class ReservationsController : Controller
    {
        private ReservationQueryService qReservationService = new ReservationQueryService();
        private ReservationCommandService cReservationService = new ReservationCommandService();

        private RestaurantQueryService qRestaurantService = new RestaurantQueryService();
        private RestaurantCommandService cRestaurantService = new RestaurantCommandService();

        // GET: Reservations
        public ActionResult List()
        {
            return View();
        }

        public JsonResult ListRefresh()
        {
            var reservations = qReservationService.GetReservations();
            var restaurants = qRestaurantService.GetRestaurants();
            var isAdmin = User.IsInRole("AppAdmin");
            return new JsonResult { Data = new { Reservations = reservations, Restaurants = restaurants, isAdmin = isAdmin }, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
