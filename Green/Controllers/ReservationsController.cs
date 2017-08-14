using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Green.Services;

namespace Green.Controllers
{
    public class ReservationsController : Controller
    {
        private QueryService qService = new QueryService();
        private CommandService cService = new CommandService();

        // GET: Reservations
        public ActionResult List()
        {
            return View();
        }

        //public JsonResult ListRefresh()
        //{

        //}
    }
}