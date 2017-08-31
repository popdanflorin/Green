using Green.Entities;
using Green.Services;
using Green.Models;
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

        public ActionResult List()
        {
            return View();
        }

        public JsonResult ListRefresh()
        {
            var meals = qMealService.GetMeals();
            var mealTypes = qMealService.GetMealTypes();
            var foods = qFoodService.GetFoods();
            var foodTypes = qFoodService.GetFoodTypes();
            return new JsonResult() { Data = new { Meals = meals, Types = mealTypes, Foods = foods, FoodTypes = foodTypes }, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult GetIngredients(string mealId)
        {
            var message = qMealService.GetIngredientsForMeal(mealId);
            return new JsonResult() { Data = message, ContentEncoding = Encoding.UTF8 };
        }

        [HttpPost]
        public JsonResult GetImage(string mealId)
        {
            var message = qMealService.GetMealImage(mealId);
            return new JsonResult() { Data = message, ContentEncoding = Encoding.UTF8 };
        }

        [HttpPost]
        public JsonResult Save(Meal meal, List<Food> ingredients)
        {
            var message = cMealService.SaveMeal(meal, ingredients);
            return new JsonResult() { Data = message, ContentEncoding = Encoding.UTF8 };
        }

        [HttpPost]
        public JsonResult Delete(string mealId)
        {
            var message = cMealService.DeleteMeal(mealId);
            return new JsonResult() { Data = message, ContentEncoding = Encoding.UTF8 };
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file, string mealId)
        {
            if (file != null)
            {
                string physicalPath;

                // delete old image from database and folder (if exists)
                var oldImageName = cMealService.DeleteImage(mealId);
                if (oldImageName != null)
                {
                    physicalPath = Server.MapPath("~/Content/images/" + oldImageName);
                    System.IO.File.Delete(physicalPath);
                }

                string ImageName = System.IO.Path.GetFileName(file.FileName);
                physicalPath = Server.MapPath("~/Content/images/" + ImageName);

                // save image in folder
                file.SaveAs(physicalPath);

                //save new record in database
                cMealService.SaveImage(ImageName, mealId);
            }
            //Display records
            return RedirectToAction("List");
        }
    }
}