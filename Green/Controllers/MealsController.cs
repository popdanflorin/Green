using Green.Entities;
using Green.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Green.Controllers
{
    public class MealsController : Controller
    {
        private MealQueryService qMealService = new MealQueryService();
        private MealCommandService cMealService = new MealCommandService();

        private FoodQueryService qFoodService = new FoodQueryService();
        private FoodCommandService cFoodService = new FoodCommandService();

        // GET: Foods
        public ActionResult List()
        {
            return View();
        }

        public JsonResult ListRefresh()
        {
            var meals = qMealService.GetMeals();
            var mealTypes = qMealService.GetMealTypes();
            var foods = qFoodService.GetFoods();
            var mealRatings = qMealService.GetMealRatings();
            return new JsonResult() { Data = new { Meals = meals, Types = mealTypes, Foods = foods, Ratings = mealRatings }, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult Save(Meal meal)
        {
            var message = cMealService.SaveMeal(meal);
            return new JsonResult() { Data = message, ContentEncoding = Encoding.UTF8 };
        }

        [HttpPost]
        public JsonResult Delete(string mealId)
        {
            var message = cMealService.DeleteMeal(mealId);
            return new JsonResult() { Data = message, ContentEncoding = Encoding.UTF8 };
        }
    }
}