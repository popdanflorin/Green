using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Green.Entities;
using Green.Entities.Enums;

namespace Green.Services
{
    public interface IMealQueryService
    {
        List<EnumItem> GetMealTypes();
        List<Meal> GetMeals();
        String GetMealImage(string mealId);
        List<MealIngredient> GetMealIngredients();
        List<MealIngredient> GetMealIngredientsForMeal(string mealId);
        List<MealIngredientDisplay> GetAllIngredients();
        List<MealIngredientDisplay> GetIngredientsForMeal(string mealId);
    }
}
