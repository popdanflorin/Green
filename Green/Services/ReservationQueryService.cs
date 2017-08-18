using Green.Entities;
using Green.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Green.Services
{
    public class ReservationQueryService
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();
        public List<Reservation> GetReservations()
        {
            var reservations = ctx.Reservations.Include("Restaurant").Include("User");
            return reservations.ToList();
        }
    }
}
