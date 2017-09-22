using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Green.Models;

namespace Green
{
    public class DataHub : Hub
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();

        public void RefreshFoods()
        {
            Clients.All.RefreshFoods();
        }
        public void RefreshMeals()
        {
            Clients.All.RefreshMeals();
        }
        public void NotifyNewReservation(string restaurantId)
        {
            string restaurantName = ctx.Restaurants.FirstOrDefault(r => r.id == restaurantId).Name;
            Clients.All.notifyNewReservation(restaurantName);
            NotifyReservationChange(restaurantId, "A new reservation has been made at ");
        }
        public void NotifyReservationChange(string restaurantId, string message)
        {
            string restaurantName = ctx.Restaurants.FirstOrDefault(r => r.id == restaurantId).Name;
            string temp = message + restaurantName + "!";
            Clients.All.notifyReservationChange(temp);
        }
    }
}