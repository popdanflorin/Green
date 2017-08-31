using Green.Entities;
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

        // GET: Menus
        public ActionResult List()
        {
            return View();
        }
        public JsonResult ListRefresh()
        {
            var menus = qMenuService.GetMenus();
            return new JsonResult() { Data = new { Menus = menus }, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}