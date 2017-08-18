using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Green.Migrations;
using Green.Entities.Enums;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using Green.Models;

namespace Green.Entities
{
    public class Reservation
    {
        
        public string Id { get; set; }
        public string RestaurantId { get; set; }
        public string ClientId { get; set; }
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
        [ForeignKey("ClientId")]
        public virtual ApplicationUser User { get; set; }
    }
}