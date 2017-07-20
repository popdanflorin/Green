using Green.Entities;
using Green.Entities.Enums;
using Green.Models;
using Green.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Green.Controllers
{
    public class FoodsController : Controller
    {
        private QueryService qService = new QueryService();
        private CommandService cService = new CommandService();

        // GET: Foods
        public ActionResult List()
        {
            var model = new FoodModel();
            model.Foods = qService.GetFoods();
            model.FoodTypes = Enum.GetValues(typeof(FoodType)).Cast<FoodType>().Select(x => new { Id = x, Description = x.ToString() }).ToList<object>();
            return View(model);
        }

        public JsonResult ListRefresh()
        {
            var foods = qService.GetFoods();
            var foodTypes = Enum.GetValues(typeof(FoodType)).Cast<FoodType>().Select(x => new { Id = x, Description = x.ToString() }).ToList<object>();
            return new JsonResult() { Data = new { Foods = foods , FoodTypes = foodTypes }, ContentEncoding = Encoding.UTF8 };
        }

        [HttpPost]
        public JsonResult Save(Food food)
        {
            var message  = cService.SaveFood(food);
            return new JsonResult() { Data = message, ContentEncoding = Encoding.UTF8 };
        }

        [HttpPost]
        public JsonResult Delete(string foodId)
        {
            var message = cService.DeleteFood(foodId);
            return new JsonResult() { Data = message, ContentEncoding = Encoding.UTF8 };
        }
    }
}
