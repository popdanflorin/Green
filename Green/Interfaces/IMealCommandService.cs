using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Green.Entities;

namespace Green.Services
{
    public interface IMealCommandService
    {
        string SaveMeal(Meal meal, List<MealIngredientDisplay> allIngredients);
        string DeleteMeal(string mealId);
        void SaveImage(string ImageName, string mealId);
        string DeleteImage(string mealId);
    }
}
