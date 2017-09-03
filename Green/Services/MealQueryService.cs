using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using Green.Entities;
using Green.Models;
using Green.Entities.Enums;

namespace Green.Services
{
    public class MealQueryService
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();

        public List<EnumItem> GetMealTypes()
        {
            return Enum.GetValues(typeof(MealType)).Cast<MealType>().Select(x => new EnumItem() { Id = (int)x, Description = x.ToString() }).ToList();
        }

        public List<Meal> GetMeals()
        {
            return ctx.Meals.ToList();
        }

        public String GetMealImage(string mealId)
        {
            var image = ctx.Meals.FirstOrDefault(m => m.Id == mealId);
            //var image = ctx.Images.FirstOrDefault(i => i.MealId == mealId);
            if (image != null)
                return image.Name;
            return null;
        }

        public List<MealIngredient> GetMealIngredients()
        {
            return ctx.MealIngredients.Include("Meal").Include("Food").ToList();
        }

        public List<MealIngredient> GetMealIngredientsForMeal(string mealId)
        {
            return GetMealIngredients().Where(e => e.MealId == mealId).ToList();
        }

        public List<Food> GetIngredientsForMeal(string mealId)
        {
            var tmp = GetMealIngredientsForMeal(mealId).Select(e => e.Food).ToList();
            tmp.Sort((e1, e2) => e1.Name.CompareTo(e2.Name));
            return tmp;
        }
    }
}
