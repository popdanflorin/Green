using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Green.Entities;
using Green.Models;
using Green.Entities.Enums;

namespace Green.Services
{
    public class MealCommandService
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();
        private const string SuccessMessage = "Action sucessfully performed.";
        private const string ErrorMessage = "An application exception occured performing action.";
        private const string ItemNotFoundMessage = "The item was not found.";

        public string SaveMeal(Meal meal, List<Food> ingredients)
        {
            try
            {
                var oldMeal = ctx.Meals.FirstOrDefault(f => f.Id == meal.Id);
                if (oldMeal == null)
                {
                    meal.Id = Guid.NewGuid().ToString();
                    ctx.Meals.Add(meal);
                }
                else
                {
                    oldMeal.Description = meal.Description;
                    oldMeal.Type = meal.Type;
                    DeleteAllIngredients(meal.Id);
                }

                if (ingredients != null)
                    SaveIngredients(meal.Id, ingredients);
                ctx.SaveChanges();
                return SuccessMessage;
            }
            catch
            {
                return ErrorMessage;
            }
        }

        public string DeleteMeal(string mealId)
        {
            try
            {
                var meal = ctx.Meals.FirstOrDefault(f => f.Id == mealId);
                if (meal != null)
                {
                    DeleteAllIngredients(mealId);
                    ctx.Meals.Remove(meal);
                    ctx.SaveChanges();
                    return SuccessMessage;
                }
                return ItemNotFoundMessage;
            }
            catch (Exception)
            {
                return ErrorMessage;
            }
        }
        
        public void SaveIngredients(string mealId, List<Food> ingredients)
        {
            var pairs = ingredients.Select(i => new MealIngredient(Guid.NewGuid().ToString(), mealId, i.Id)).ToList();
            pairs.ForEach(p => ctx.MealIngredients.Add(p));
        }

        public void DeleteAllIngredients(string mealId)
        {
            var pairs = ctx.MealIngredients.Where(e => e.MealId == mealId).ToList();
            pairs.ForEach(p => ctx.MealIngredients.Remove(p));
        }
    }
}
