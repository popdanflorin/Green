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
        private QueryService qService = new QueryService();
        private CommandService cService = new CommandService();

        // GET: Foods
        public ActionResult List()
        {
            return View();
        }

        public JsonResult ListRefresh()
        {
            var meals = qService.GetMeals();
            var mealTypes = qService.GetMealTypes();
            var mealStatuses = qService.GetMealStatuses();
            var mealRatings = qService.GetMealRatings();
            return new JsonResult() { Data = new { Meals = meals, Types = mealTypes, Statuses = mealStatuses, Ratings = mealRatings }, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult Save(Meal meal)
        {
            var message = cService.SaveMeal(meal);
            return new JsonResult() { Data = message, ContentEncoding = Encoding.UTF8 };
        }

        [HttpPost]
        public JsonResult Delete(string mealId)
        {
            var message = cService.DeleteMeal(mealId);
            return new JsonResult() { Data = message, ContentEncoding = Encoding.UTF8 };
        }
    }
}