using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Green.Controllers
{
    public class ReservationsController : Controller
    {
        // GET: Reservations
        public ActionResult List()
        {
            return View();
        }
    }
}