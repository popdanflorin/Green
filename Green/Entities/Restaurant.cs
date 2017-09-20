using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Green.Models;

namespace Green.Entities
{
    public class Restaurant
    {
        public string id { get; set; }
        public string OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public virtual ApplicationUser User { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public Enums.RestaurantType Type { get; set; }
        public int SeatsAvailable { get; set; }
        public int OpeningHour { get; set; }
        public int ClosingHour { get; set; }
      
        public string TypeDisplay
        {
            get
            {
                return Type.ToString();
            }
        }
    }
}