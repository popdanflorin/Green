using Green.Entities;
using Green.Entities.Enums;
using Green.Services;
using System;
using System.Linq;
using System.Text;
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
            return View();
        }

        public JsonResult ListRefresh()
        {
            var foods = qService.GetFoods();
            var foodTypes = qService.GetFoodTypes();
            return new JsonResult() { Data = new { Foods = foods, FoodTypes = foodTypes }, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult Save(Food food)
        {
            var message = cService.SaveFood(food);
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
