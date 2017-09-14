using Green.Entities;
using Green.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Green.Services
{
    public class MenuCommandService
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();
        private const string SuccessMessage = "Action sucessfully performed.";
        private const string ErrorMessage = "An application exception occured performing action.";
        private const string ItemNotFoundMessage = "The item was not found.";

        public string SaveMenu(Menu menu, List<MenuMealDisplay> allMeals)
        {
            try
            {
                var meals = allMeals.Where(m => m.isSelected == true).Select(m => new Meal
                {
                    Id = m.Id,
                    Description = m.Description,
                    ImageName = m.ImageName,
                    Name = m.Name,
                    Type = m.Type
                }).ToList();

                var oldMenu = ctx.Menus.FirstOrDefault(f => f.Id == menu.Id);
                if (oldMenu == null)
                {
                    menu.Id = Guid.NewGuid().ToString();
                    ctx.Menus.Add(menu);
                }
                else
                {
                    DateTime tmp = new DateTime();
                    try
                    {
                        tmp = oldMenu.StartDate;
                        oldMenu.StartDate = new DateTime(menu.StartDate.Year, menu.StartDate.Month, menu.StartDate.Day);
                        ctx.SaveChanges();
                    }
                    catch
                    {
                        oldMenu.StartDate = tmp;
                    }

                    try
                    {
                        tmp = oldMenu.EndDate;
                        oldMenu.EndDate = new DateTime(menu.EndDate.Year, menu.EndDate.Month, menu.EndDate.Day);
                        ctx.SaveChanges();
                    }
                    catch
                    {
                        oldMenu.EndDate = tmp;
                    }
                    //oldMenu.StartDate = DateTime.Now;
                    //oldMenu.EndDate = DateTime.Now;
                    DeleteAllMeals(menu.Id);
                }

                if (meals != null)
                    SaveMeals(menu.Id, meals);
                ctx.SaveChanges();
                return SuccessMessage;
            }
            catch (Exception e)
            {
                return ErrorMessage;
            }
        }

        public string DeleteMenu(string menuId)
        {
            try
            {
                var menu = ctx.Menus.FirstOrDefault(f => f.Id == menuId);
                if (menu != null)
                {
                    DeleteAllMeals(menuId);
                    ctx.Menus.Remove(menu);
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

        private void SaveMeals(string menuId, List<Meal> meals)
        {
            var pairs = meals.Select(i => new MenuMeal(Guid.NewGuid().ToString(), menuId, i.Id)).ToList();
            pairs.ForEach(p => ctx.MenuMeals.Add(p));
        }

        private void DeleteAllMeals(string menuId)
        {
            var pairs = ctx.MenuMeals.Where(e => e.MenuId == menuId).ToList();
            pairs.ForEach(p => ctx.MenuMeals.Remove(p));
        }

        public string DeleteRestaurantMenus(string restaurantId)
        {
            try
            {
                var menus = ctx.Menus.Where(m => m.RestaurantId == restaurantId).ToList();
                menus.ForEach(m => DeleteMenu(m.Id));
                return SuccessMessage;
            }
            catch (Exception)
            {
                return ErrorMessage;
            }
        }

        public string DeleteMealFromMenus(string mealId)
        {
            try
            {
                ctx.MenuMeals.Where(m => m.MealId == mealId).ToList().ForEach(m => ctx.MenuMeals.Remove(m));
                return SuccessMessage;
            }
            catch (Exception)
            {
                return ErrorMessage;
            }
        }
    }

}