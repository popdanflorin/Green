using Green.Entities;
using Green.Services;
using Green.Interfaces;
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
        private IUserFavoritesQueryService qService;
        private IUserFavoritesCommandService cService;

        public UserFavoritesController(IUserFavoritesCommandService _cService, IUserFavoritesQueryService _qService)
        {
            cService = _cService;
            qService = _qService;
        }

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
        public JsonResult UserFavoritesGet(string UserId)
        {
            var userFavorites = qService.GetUserFavorites(UserId);
            return new JsonResult() { Data = new { UserFavorites = userFavorites }, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        [HttpPost]
        public JsonResult Save(UserFavorites favorite)
        {
            var message = cService.SaveFavorite(favorite);
            return new JsonResult() { Data = message, ContentEncoding = Encoding.UTF8 };
        }
        public JsonResult Delete(string restaurantId)
        {
            var message = cService.DeleteFavorite(restaurantId);
            return new JsonResult() { Data = message, ContentEncoding = Encoding.UTF8 };
        }
    }
}