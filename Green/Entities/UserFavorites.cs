using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Green.Entities
{
    public class UserFavorites
    {
        public string Id { get; set; }
        public string RestaurantId { get; set; }
        [ForeignKey("RestaurantId")]
        public virtual Restaurant Restaurant { get; set; }
        public string ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Models.ApplicationUser Client { get; set; }
    }
}