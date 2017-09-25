using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Green.Entities;
using Green.Models;

namespace Green.Interfaces
{
    public interface IReservationQueryService
    {
        List<Reservation> GetReservations();
        List<Reservation> GetReservations(string restaurantId, int year, int month);
        List<RestaurantPercentages> GetSeatsPercetageForAllPerYear(List<Restaurant> restaurants, int year);
        List<int> GetSeatsPercetageForRestaurantPerYear(Restaurant restaurant, int year);
        List<int> GetSeatsPercentageForRestaurantPerMonth(string restaurantId, int year, int month);
    }
}
