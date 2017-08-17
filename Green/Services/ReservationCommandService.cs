using Green.Entities;
using Green.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Green.Services
{
    public class ReservationCommandService
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();
        private const string SuccessMessage = "Action sucessfully performed.";
        private const string ErrorMessage = "An application exception occured performing action.";
        private const string ItemNotFoundMessage = "The item was not found.";
        public string SaveReservation(Reservation reservation)
        {
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
                    oldReservation.RestaurantName = reservation.RestaurantName;
                    oldReservation.ClientName = reservation.ClientName;
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
    }
}
