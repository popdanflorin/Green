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
                default: return String.Empty;
            }
        }

        public static string ToString(this MealType mealType)
        {
            switch (mealType)
            {
                case MealType.Breakfast: return "Breakfast";
                case MealType.Lunch: return "Lunch";
                case MealType.Dinner: return "Dinner";
                case MealType.Snack: return "Snack";
                default: return String.Empty;
            }
        }

        public static string ToString(this RestaurantType restaurantType)
        {
            switch (restaurantType)
            {
                case RestaurantType.Backery: return "Backery";
                case RestaurantType.Pastry: return "Pastry";
                case RestaurantType.Traditional: return "Traditional";
                case RestaurantType.Vegan: return "Vegan";
                case RestaurantType.Vegetarian: return "Vegetarian";
                default: return String.Empty;
            }
        }
    }
}