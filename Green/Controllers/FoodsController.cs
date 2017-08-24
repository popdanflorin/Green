using Green.Entities;
using Green.Services;
using System.Text;
using System.Web.Mvc;

namespace Green.Controllers
{
    public class FoodsController : Controller
    {
        private FoodQueryService qService = new FoodQueryService();
        private FoodCommandService cService = new FoodCommandService();

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
