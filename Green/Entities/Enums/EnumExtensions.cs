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
                case FoodType.Dairy: return "Dairy";
                case FoodType.Sweets: return "Sweets";
                case FoodType.Vegetables: return "Vegetables";
                case FoodType.Spices: return "Spices";
                case FoodType.SaucesAndToppings: return "Sauces and Toppings";
                default: return String.Empty;
            }
        }

        public static string ToString(this MealType mealType)
        {
            switch (mealType)
            {
                case MealType.Soup: return "Soup";
                case MealType.Pizza: return "Pizza";
                case MealType.Pasta: return "Pasta";
                case MealType.Salad: return "Salad";
                case MealType.ChickenDishes: return "Chicken Dishes";
                case MealType.PorkDishes: return "Pork Dishes";
                case MealType.SideDishes: return "Side Dishes";
                case MealType.VegetarianDishes: return "Vegetarian Dishes";
                case MealType.Dessert: return "Dessert";
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
                case RestaurantType.FastFood: return "FastFood";
                case RestaurantType.Indian: return "Indian";
                case RestaurantType.Italian: return "Italian";
                default: return String.Empty;
            }
        }
    }
}