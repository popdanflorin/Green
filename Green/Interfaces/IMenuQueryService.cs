using Green.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Green.Interfaces
{
    public interface IMenuQueryService
    {
        Menu GetCurrentMenu(string restaurantId);
        List<Menu> GetMenus();
        List<Menu> GetMenus(string restaurantId);
        Menu GetMenuDetails(string menuId);
        List<MenuMealDisplay> GetAllMeals();
        List<MenuMealDisplay> GetMealsForMenu(string menuId);
        List<MenuMealDisplay> GetNewMealsForMenu(string menuId, List<MenuMealDisplay> selectedMeals);
    }
}
