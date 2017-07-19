using Green.Entities.Enums;
using Green.Models;
using Green.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Green.Controllers
{
    public class FoodsController : Controller
    {
        private QueryService qService = new QueryService();

        // GET: Foods
        public ActionResult List()
        {
            var model = new FoodModel();
            model.Foods = qService.GetFoods();
            model.FoodTypes = Enum.GetValues(typeof(FoodType)).Cast<FoodType>().Select(x => new { Id = x, Description = x.ToString() }).ToList<object>();
            return View(model);
        }
    }
}