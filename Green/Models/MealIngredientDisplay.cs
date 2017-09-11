using Green.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Green.Entities
{
    public class MealIngredientDisplay
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public FoodType Type { get; set; }

        public string TypeDisplay
        {
            get
            {
                return Type.ToString();
            }
        }
        public bool isSelected { get; set; }
    }
}