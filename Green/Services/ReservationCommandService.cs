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

        private const string SuccessMessage = "Action sucessfully performed.";
        private const string ErrorMessage = "An application exception occured performing action.";
        private const string ItemNotFoundMessage = "The item was not found.";
        private const string SeatsUnavailableMessage = "There are not enough seats available.";
        public string SaveReservation(Reservation reservation)
        {
            if (!ValidateReservation(reservation))
                return SeatsUnavailableMessage;
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
        private bool ValidateReservation(Reservation reservation)
        {
            var reservations = reservationQService.GetReservations();
            return true;
        }
    }
}
