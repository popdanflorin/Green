using System;
using System.Web;
using System.Linq;
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
        public string SaveMeal(Meal meal)
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
                    oldMeal.Rating = meal.Rating;
                    oldMeal.Ingredients = meal.Ingredients;
                }

                ctx.SaveChanges();
                return SuccessMessage;
            }
            catch
            {
                return ErrorMessage;
            }
        }

        public string DeleteMeal(string id)
        {
            try
            {
                var meal = ctx.Meals.FirstOrDefault(f => f.Id == id);
                if (meal != null)
                {
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

    }
}
