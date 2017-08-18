using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Green.Entities
{
    public class Restaurant
    {
        public string id { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public Enums.RestaurantType Type { get; set; }
        public int MaxPrice { get; set; }
        public int SeatsAvailable { get; set; }
        public int Rating { get; set; }
        public string TypeDisplay
        {
            get
            {
                return Type.ToString();
            }
        }
        public string  MealId { get; set; }
        [ForeignKey("MealId")]
        public virtual Meal Meal { get; set; }
    }
}