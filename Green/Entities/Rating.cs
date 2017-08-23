using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Green.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Green.Entities
{
    public class Rating
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string RestaurantId { get; set; }
        public int Value { get; set; }
        [ForeignKey("ClientId")]
        public virtual ApplicationUser Client { get; set; }
        [ForeignKey("RestaurantId")]
        public virtual Restaurant Restaurant { get; set; }
    }
}