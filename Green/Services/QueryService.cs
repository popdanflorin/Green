using Green.Entities;
using Green.Entities.Enums;
using Green.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Green.Services
{
    public class QueryService
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();

        public List<Food> GetFoods()
        {
            return ctx.Foods.ToList();
        }

        public List<EnumItem> GetFoodTypes()
        {
            return Enum.GetValues(typeof(FoodType)).Cast<FoodType>().Select(x => new EnumItem() { Id = (int)x, Description = x.ToString() }).ToList();
        }

        public List<EnumItem> GetMealTypes()
        {
            return Enum.GetValues(typeof(MealType)).Cast<MealType>().Select(x => new EnumItem() { Id = (int)x, Description = x.ToString() }).ToList();
        }

        public List<EnumItem> GetMealStatuses()
        {
            return Enum.GetValues(typeof(MealStatus)).Cast<MealStatus>().Select(x => new EnumItem() { Id = (int)x, Description = x.ToString() }).ToList();
        }

        public List<EnumItem> GetMealRatings()
        {
            return Enum.GetValues(typeof(MealRating)).Cast<MealRating>().Select(x => new EnumItem() { Id = (int)x, Description = x.ToString() }).ToList();
        }

        public List<Meal> GetMeals()
        {
            return ctx.Meals.ToList();
        }

        public List<Reservation> GetReservations()
        {
            return ctx.Reservations.ToList();
        }
    }
}