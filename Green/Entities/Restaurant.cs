using System;
using System.Collections.Generic;
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
        public string TypeDisplay
        {
            get
            {
                return Type.ToString();
            }
        }
    }
}