using Green.Entities;
using Green.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Green.Models
{
    public class FoodModel
    {
        public List<Food> Foods { get; set; }
        public List<object> FoodTypes { get; set; }
    }
}