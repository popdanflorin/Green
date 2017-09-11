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

        // returns all ingredients and set isSelected to false for all
        public List<MealIngredientDisplay> GetAllIngredients()
        {
            var tmp = ctx.Foods.Select(m => new MealIngredientDisplay
            {
                Id = m.Id,
                Name = m.Name,
                Type = m.Type,
                isSelected = false
            }).ToList();
            tmp.OrderBy(m => m.TypeDisplay);
            return tmp;
        }

        // returns all ingredients and set isSelected to true if the ingredient is in the current meal, otherwise it is set to false
        public List<MealIngredientDisplay> GetIngredientsForMeal(string mealId)
        {
            var allIngredients = GetAllIngredients();
            var mealIngredients = ctx.MealIngredients.Where(m => m.MealId == mealId).ToList();
            if (!mealIngredients.Any())
                return allIngredients;
            mealIngredients.ForEach(m => allIngredients.FirstOrDefault(ingredient => ingredient.Id == m.FoodId).isSelected = true);
            return allIngredients;
        }
    }
}
