using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Green.Entities.Enums
{
    public static class EnumExtensions
    {
        public static string ToString(this FoodType foodType)
        {
            switch (foodType)
            {
                case FoodType.Alcohol: return "Alcohol";
                case FoodType.Cereals: return "Cereals";
                case FoodType.Fats: return "Fats";
                case FoodType.Fruits: return "Fruits";
                case FoodType.Meats: return "Meats";
                case FoodType.Milk: return "Milk";
                case FoodType.Sweets: return "Sweets";
                case FoodType.Vegetables: return "Vegetables";
                default:return String.Empty;
            }
        }
    }
}