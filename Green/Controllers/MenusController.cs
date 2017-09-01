﻿using Green.Entities;
using Green.Services;
using Green.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;

namespace Green.Controllers
{
    public class MenusController : Controller
    {
        private MenuQueryService qMenuService = new MenuQueryService();
        private MenuCommandService cMenuService = new MenuCommandService();

        private MealQueryService qMealService = new MealQueryService();
        private MealCommandService cMealService = new MealCommandService();

        // GET: Menus
        public ActionResult List()
        {
            return View();
        }
        public JsonResult ListRefresh()
        {
            var meals = qMealService.GetMeals();
            var mealTypes = qMealService.GetMealTypes();
            return new JsonResult() { Data = new { Meals = meals, MealTypes = mealTypes}, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult Save(Menu menu, List<Meal> meals)
        {
            var message = cMenuService.SaveMenu(menu, meals);
            return new JsonResult() { Data = message, ContentEncoding = Encoding.UTF8 };
        }

        [HttpPost]
        public JsonResult Delete(string menuId)
        {
            var message = cMenuService.DeleteMenu(menuId);
            return new JsonResult() { Data = message, ContentEncoding = Encoding.UTF8 };
        }
        
        public JsonResult GetMenu(string restaurantId)
        {
            var menu = qMenuService.GetMenu(restaurantId);
            var restaurantMeals = qMenuService.GetMealsForMenu(menu.Id);
            return new JsonResult() { Data = new { Id = menu.Id, StartDate = menu.StartDate.Date, EndDate = menu.EndDate.Date, RestaurantMeals =  restaurantMeals }, ContentEncoding = Encoding.UTF8 };
        }

        public JsonResult GetMeals()
        {
            var meals = qMenuService.GetAllMeals();
            return new JsonResult() { Data = new { Meals = meals }, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetMealsForMenu(string menuId)
        {
            var meals = qMenuService.GetMealsForMenu(menuId);
            return new JsonResult() { Data = new { Meals = meals }, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}