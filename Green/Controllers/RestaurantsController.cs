using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Green.Entities;
using Green.Entities.Enums;
using Green.Services;
using System.Text;
using Green.Models;
using System.IO;

namespace Green.Controllers
{
    public class RestaurantsController : Controller
    {
        private RestaurantQueryService qService = new RestaurantQueryService();
        private RestaurantCommandService cService = new RestaurantCommandService();
        private const string ErrorMessage = "An application exception occured performing action.";
        // GET: Restaurants

        [Authorize (Roles = "AppAdmin")]
        public ActionResult List()
        {
            return View();
        }
        public ActionResult UserRestaurants()
        {
            return View();
        }
        public JsonResult ListRefresh()
        {
            var restaurants = qService.GetRestaurants();
            var restaurantTypes = qService.GetRestaurantTypes();
            return new JsonResult() { Data = new { Restaurants = restaurants, RestaurantTypes = restaurantTypes }, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult Save(Restaurant restaurant)
        {
            var message = cService.SaveRestaurant(restaurant);
            return new JsonResult() { Data = message, ContentEncoding = Encoding.UTF8 };
        }
        public JsonResult Delete(string restaurantId)
        {
            var message = cService.DeleteRestaurant(restaurantId);
            return new JsonResult() {Data=message,ContentEncoding=Encoding.UTF8};
        }
        //upload images
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file,string restaurantId)
        {
            if (file != null)
            {
                ApplicationDbContext db = new ApplicationDbContext();
                string ImageName = System.IO.Path.GetFileName(file.FileName);
                string physicalPath = Server.MapPath("~/Content/images/" + ImageName);

                // save image in folder
                file.SaveAs(physicalPath);

                //save new record in database
                Image newRecord = new Image();
                newRecord.Id = Guid.NewGuid().ToString();
                newRecord.Name = ImageName;

                newRecord.RestaurantId = restaurantId;
                db.Images.Add(newRecord);
                db.SaveChanges();

            }
            //Display records
            return RedirectToAction("UserRestaurants");
        }
    }
    
}