using Green.Entities;
using Green.Interfaces;
using Green.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Green.Services
{
    public class MenuQueryService : IMenuQueryService
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();

        public Menu GetCurrentMenu(string restaurantId)
        {
            var menus = GetMenus(restaurantId);
            if (!menus.Any())
                return null;
            var currentDate = new DateTime();
            return menus.FirstOrDefault(menu =>
                menu.StartDate.Year <= currentDate.Year && menu.EndDate.Year >= currentDate.Year &&
                menu.StartDate.Month <= currentDate.Month && menu.EndDate.Month >= currentDate.Month &&
                menu.StartDate.Day <= currentDate.Day && menu.EndDate.Day >= currentDate.Day
            );
        }

        public List<Menu> GetMenus()
        {
            return ctx.Menus.ToList();
        }

        public List<Menu> GetMenus(string restaurantId)
        {
            return ctx.Menus.Where(m => m.RestaurantId == restaurantId).OrderBy(m => m.StartDate).ToList();
        }

        public Menu GetMenuDetails(string menuId)
        {
            return ctx.Menus.FirstOrDefault(m => m.Id == menuId);
        }

        // returns all meals and set isSelected to false for all
        public List<MenuMealDisplay> GetAllMeals()
        {
            //var menuMeals = ctx.MenuMeals.ToList();
            //if (!menuMeals.Any())
            //    return null;
            //var meals = menuMeals.Select(m => m.Meal).ToList();
            //if (!meals.Any())
            //    return null;
            //var tmp = meals.Select(m => new MenuMealDisplay {
            //    Id = m.Id,
            //    Description = m.Description,
            //    ImageName = m.ImageName, 
            //    Name = m.Name,
            //    Type = m.Type,
            //    isSelected = false
            //}).ToList();
            //return tmp;
            var tmp = ctx.Meals.Select(m => new MenuMealDisplay
            {
                Id = m.Id,
                Description = m.Description,
                ImageName = m.ImageName,
                Name = m.Name,
                Type = m.Type,
                isSelected = false
            }).OrderBy(m => m.Type).ThenBy(m => m.Name).ToList();
            return tmp;
        }

        // returns all meals and set isSelected to true if the meal is in the current menu, otherwise it is set to false
        public List<MenuMealDisplay> GetMealsForMenu(string menuId)
        {
            var allMeals = GetAllMeals();
            var menuMeals = ctx.MenuMeals.Where(m => m.MenuId == menuId).ToList();
            if (!menuMeals.Any())
                return allMeals;
            menuMeals.ForEach(m => allMeals.FirstOrDefault(meal => meal.Id == m.MealId).isSelected = true);
            return allMeals;
        }

        public List<MenuMealDisplay> GetNewMealsForMenu(string menuId, List<MenuMealDisplay> selectedMeals)
        {
            var allMeals = GetAllMeals();
            var menuMeals = selectedMeals.Where(m => m.isSelected == true).ToList();
            if (!menuMeals.Any())
                return allMeals;
            menuMeals.ForEach(m => allMeals.FirstOrDefault(meal => meal.Id == m.Id).isSelected = true);
            return allMeals;
        }
    }
}