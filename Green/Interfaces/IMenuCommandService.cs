using Green.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Green.Interfaces
{
    public interface IMenuCommandService
    {
        string SaveMenu(Menu menu, List<MenuMealDisplay> allMeals);
        string DeleteMenu(string menuId);
        void SaveMeals(string menuId, List<Meal> meals);
        void DeleteAllMeals(string menuId);
        string DeleteRestaurantMenus(string restaurantId);
        string DeleteMealFromMenus(string mealId);
        bool ValidateMenuDate(Menu menu);
    }
}
