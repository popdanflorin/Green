using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Green.Entities
{
    public class Reservation
    {
        
        public string Id { get; set; }
        
        public string RestaurantId { get; set; }
        public string ClientName { get; set; }
        public DateTime ReservationDate { get; set; }
        public string ReservationDateDisplay
        {
            get
            {
                return ReservationDate.ToString("dd-MMM-yyyy HH:mm");
            }
        }
        public string Seats { get; set; }

        [ForeignKey("RestaurantId")]
        public virtual Restaurant Restaurant { get; set; }
    }
}