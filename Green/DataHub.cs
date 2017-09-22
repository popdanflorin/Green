using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Green
{
    public class DataHub : Hub
    {
        public void RefreshFoods()
        {
            Clients.All.RefreshFoods();
            Clients.All.RefreshFoodsForMeals();
        }
        public void RefreshMeals()
        {
            Clients.All.RefreshMeals();
        }
    }
}