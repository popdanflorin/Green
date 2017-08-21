using Green.Entities;
using Green.Models;
using Green.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Green.Services
{
    public class ReservationCommandService
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();
        private ReservationQueryService reservationQService = new ReservationQueryService();
        private RestaurantQueryService restaurantQService = new RestaurantQueryService();

        private const string SuccessMessage = "Action sucessfully performed.";
        private const string ErrorMessage = "An application exception occured performing action.";
        private const string ItemNotFoundMessage = "The item was not found.";
        private const string SeatsUnavailableMessage = "There are not enough seats available.";
        private const string TimeUnavailableMessage = "The selected time is not available.";
        public string SaveReservation(Reservation reservation)
        {
            string message = "";
            if (!ValidateReservationHour(reservation))
                message += TimeUnavailableMessage;
            if (!ValidateReservationSeats(reservation))
                message += SeatsUnavailableMessage;
            if (message != "")
                return message;

            try
            {
                var oldReservation = ctx.Reservations.FirstOrDefault(r => r.Id == reservation.Id);              
                if (oldReservation == null)
                {
                    reservation.Id = Guid.NewGuid().ToString();
                    ctx.Reservations.Add(reservation);
                }
                else
                {
                    oldReservation.RestaurantId = reservation.RestaurantId;
                    oldReservation.ClientId = reservation.ClientId;
                    oldReservation.ReservationDate = reservation.ReservationDate;
                    oldReservation.Seats = reservation.Seats;
                }

                ctx.SaveChanges();
                return SuccessMessage;
            }
            catch (Exception)
            {
                return ErrorMessage;
            }
        }
        public string DeleteReservation(string id)
        {
            try
            {
                var reservation = ctx.Reservations.FirstOrDefault(r => r.Id == id);
                if (reservation != null)
                {
                    ctx.Reservations.Remove(reservation);
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
        private bool ValidateReservationSeats(Reservation reservation)
        {
            var reservations = reservationQService.GetReservations().Where(r => r.RestaurantId == reservation.RestaurantId && r.ReservationDate == reservation.ReservationDate);
            var restaurant = restaurantQService.GetRestaurants().FirstOrDefault(r => r.id == reservation.RestaurantId);
 
            var unavailableSeats = reservations.Sum(r => Int32.Parse(r.Seats));
            var oldReservation = ctx.Reservations.FirstOrDefault(r => r.Id == reservation.Id);
            if (oldReservation != null)
                unavailableSeats -= Int32.Parse(oldReservation.Seats);
            if (restaurant.SeatsAvailable - unavailableSeats < Int32.Parse(reservation.Seats))
                return false;
            return true;
        }

        private bool ValidateReservationHour(Reservation reservation)
        {
            var restaurant = restaurantQService.GetRestaurants().FirstOrDefault(r => r.id == reservation.RestaurantId);
            
            if (reservation.ReservationDate.Hour < restaurant.OpeningHour || reservation.ReservationDate.Hour > restaurant.ClosingHour)
                return false;
            return true;
        }
    }
}
