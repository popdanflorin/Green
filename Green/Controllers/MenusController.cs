using Green.Entities;
using Green.Services;
using Green.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using Green.Interfaces;

namespace Green.Controllers
{
    public class MenusController : Controller
    {
        private IMenuQueryService qMenuService;
        private IMenuCommandService cMenuService;

        private IMealQueryService qMealService;
        private IMealCommandService cMealService;

        public MenusController(IMenuCommandService _cMenuService, IMenuQueryService _qMenuService, IMealCommandService _cMealService, IMealQueryService _qMealService)
        {
            cMenuService = _cMenuService;
            qMenuService = _qMenuService;
            cMealService = _cMealService;
            qMealService = _qMealService;
        }

        // GET: Menus
        public ActionResult List()
        {
            return View();
        }
        public JsonResult ListRefresh(string restaurantId)
        {
            var menus = qMenuService.GetMenus(restaurantId);
            var mealTypes = qMealService.GetMealTypes();
            return new JsonResult() { Data = new { Menus = menus, MealTypes = mealTypes }, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult Save(Menu menu, List<MenuMealDisplay> meals)
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

        public JsonResult GetNewMealsForMenu(string menuId, List<MenuMealDisplay> selectedMeals)
        {
            var meals = qMenuService.GetNewMealsForMenu(menuId, selectedMeals);
            return new JsonResult() { Data = new { Meals = meals }, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}