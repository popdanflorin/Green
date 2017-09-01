using Green.Entities;
using Green.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Green.Services
{
    public class MenuQueryService
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();

        public List<Menu> GetMenus()
        {
            return ctx.Menus.ToList();
        }

        public Menu GetMenu(string restaurantId)
        {
            Menu menu = ctx.Menus.FirstOrDefault(m => m.RestaurantId == restaurantId);
            if (menu == null)
            {
                menu = new Menu();
                menu.Id = null;
                menu.StartDate = new DateTime();
                menu.EndDate = new DateTime();
            }
            return menu;
        }

        public List<Meal> GetAllMeals()
        {
            var menuMeals = ctx.MenuMeals.ToList();
            if (!menuMeals.Any())
                return null;
            var meals = menuMeals.Select(m => m.Meal).ToList();
            if (!meals.Any())
                return null;
            return meals;
        }

        public List<Meal> GetMealsForMenu(string menuId)
        {
            var menuMeals = ctx.MenuMeals.Where(m => m.MenuId == menuId).ToList();
            if (!menuMeals.Any())
                return null;
            var meals = menuMeals.Select(m => m.Meal).ToList();
            if (!meals.Any())
                return null;
            return meals;
        }
    }
}