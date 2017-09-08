using Green.Entities;
using Green.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Green.Controllers
{
    public class UserFavoritesController : Controller
    {

        private UserFavoritesQueryService qService = new UserFavoritesQueryService();
        private UserFavoritesCommandService cService = new UserFavoritesCommandService();
        // GET: UserFavorites
        public ActionResult List()
        {
            return View();
        }
        public JsonResult GetUserId()
        {
            var userId = User.Identity.GetUserId();
            return new JsonResult { Data = new { UserId = userId }, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult Save(UserFavorites favorite)
        {
            var message = cService.SaveFavorite(favorite);
            return new JsonResult() { Data = message, ContentEncoding = Encoding.UTF8 };
        }
    }
}