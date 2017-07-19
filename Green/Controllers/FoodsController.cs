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
        public ActionResult Index()
        {
            var data = qService.GetFoods();
            return View(data);
        }
    }
}