using Green.Entities;
using Green.Interfaces;
using Green.Models;
using Green.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Green.Services
{
    public class ReservationCommandService : IReservationCommandService
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();

        private IReservationQueryService reservationQService;
        private IRestaurantQueryService restaurantQService;

        private const string SuccessMessage = "Action sucessfully performed.";
        private const string ErrorMessage = "An application exception occured performing action.";
        private const string ItemNotFoundMessage = "The item was not found.";
        private const string SeatsUnavailableMessage = "There are not enough seats available.";
        private const string TimeUnavailableMessage = "The selected time is not available.";

        public ReservationCommandService(IReservationQueryService _reservationQService, IRestaurantQueryService _restaurantQService)
        {
            reservationQService = _reservationQService;
            restaurantQService = _restaurantQService;
        }

        public string SaveReservation(Reservation reservation)
        {
            try
            {
                var oldReservation = ctx.Reservations.FirstOrDefault(r => r.Id == reservation.Id);
                if (oldReservation == null)
                {
                    string message = "";
                    if (!ValidateReservationHour(reservation))
                        message += TimeUnavailableMessage;
                    if (!ValidateReservationSeats(reservation))
                        message += SeatsUnavailableMessage;
                    if (message != "")
                        return message;
                    reservation.Id = Guid.NewGuid().ToString();
                    ctx.Reservations.Add(reservation);
                }
                else
                {
                    DateTime tmp = new DateTime();
                    try
                    {
                        tmp = oldReservation.ReservationDate;
                        oldReservation.ReservationDate = new DateTime(reservation.ReservationDate.Year, reservation.ReservationDate.Month, reservation.ReservationDate.Day);
                        ctx.SaveChanges();
                    }
                    catch
                    {
                        oldReservation.ReservationDate = tmp;
                    }
                    string message = "";
                    if (!ValidateReservationHour(oldReservation))
                        message += TimeUnavailableMessage;
                    if (!ValidateReservationSeats(reservation))
                        message += SeatsUnavailableMessage;
                    if (message != "")
                    {
                        oldReservation.ReservationDate = tmp;
                        return message;
                    }
                    oldReservation.RestaurantId = reservation.RestaurantId;
                    oldReservation.ClientId = reservation.ClientId;
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

        public string DeleteReservationForRestaurant(string restaurantId)
        {
            try
            {
                var reservations = ctx.Reservations.Where(r => r.RestaurantId == restaurantId).ToList();
                if (reservations.Any())
                {
                    ctx.Reservations.RemoveRange(reservations);
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
        public bool ValidateReservationSeats(Reservation reservation)
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

        public bool ValidateReservationHour(Reservation reservation)
        {
            var restaurant = restaurantQService.GetRestaurants().FirstOrDefault(r => r.id == reservation.RestaurantId);

            if (reservation.ReservationDate.Hour < restaurant.OpeningHour || reservation.ReservationDate.Hour > restaurant.ClosingHour)
                return false;
            return true;
        }
    }
}
