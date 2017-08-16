using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Green.Entities
{
    public class Reservation
    {
        
        public string Id { get; set; }
        public string RestaurantName { get; set; }
        public string ClientName { get; set; }
        public DateTime ReservationDate { get; set; }
        public string ReservationDateDisplay
        {
            get
            {
                return ReservationDate.ToString("dd-MMM-yyyy HH:mm");
            }
        }
        public int Seats { get; set; }
    }
}