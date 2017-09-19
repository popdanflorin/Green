using Green.Entities;
using Green.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Green.Services
{
    public class ReservationQueryService
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();
        public List<Reservation> GetReservations()
        {
            return ctx.Reservations.Include("Restaurant").Include("User").OrderBy(r => r.ReservationDate.Year).ThenBy(r => r.ReservationDate.Month).ThenBy(r => r.ReservationDate.Day).ThenBy(r => r.ReservationDate.Hour).ToList();
        }

        public List<Reservation> GetReservations(string restaurantId, int year, int month)
        {
            return GetReservations().Where(r => r.RestaurantId == restaurantId && r.ReservationDate.Year == year && r.ReservationDate.Month == month).ToList();
        }

        public List<RestaurantPercentages> GetSeatsPercetageForAllPerYear(List<Restaurant> restaurants, int year)
        {
            List<RestaurantPercentages> allPercentages = new List<RestaurantPercentages>();
            restaurants.ForEach(r => {
                allPercentages.Add(new RestaurantPercentages { Restaurant = r, Percentages = GetSeatsPercetageForRestaurantPerYear(r, year) });
            });
            return allPercentages;
        }
        public List<int> GetSeatsPercetageForRestaurantPerYear(Restaurant restaurant, int year)
        {
            List<int> percentages = new List<int>();
            int hours = restaurant.ClosingHour - restaurant.OpeningHour;
            int days, max, current;
            if (hours == 0)
                hours = 24;
            var allReservations = ctx.Reservations.Where(r => r.RestaurantId == restaurant.id && r.ReservationDate.Year == year).ToList();
                      
            for (var month = 1; month <= 12; ++month)
            {
                switch (month)
                {
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 8:
                    case 10:
                    case 12:
                        days = 31;
                        break;
                    case 2:
                        days = year % 4 == 0 ? 29 : 28;
                        break;
                    default:
                        days = 30;
                        break;
                }
                max = days * hours * restaurant.SeatsAvailable;
                var reservations = allReservations.Where(r => r.ReservationDate.Month == month).ToList();
                current = 0;
                reservations.ForEach(r => current += Int32.Parse(r.Seats));
                int percentage = current * 100 / max;
                percentages.Add(percentage);
            }
            return percentages;
        }

        public List<int> GetSeatsPercentageForRestaurantPerMonth(string restaurantId, int year, int month)
        {
            Restaurant restaurant = ctx.Restaurants.FirstOrDefault(r => r.id == restaurantId);
            int hours = restaurant.ClosingHour - restaurant.OpeningHour;
            int days;
            if (hours == 0)
                hours = 24;
            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    days = 31;
                    break;
                case 2:
                    days = year % 4 == 0 ? 29 : 28;
                    break;
                default:
                    days = 30;
                    break;
            }
            List<int> percentages = new List<int>();
            var reservations = ctx.Reservations.Where(r => r.RestaurantId == restaurantId && r.ReservationDate.Year == year && r.ReservationDate.Month == month).ToList();

            int max = hours * restaurant.SeatsAvailable, current;
            for (var i = 1; i <= days; ++i)
            {
                current = 0;
                var todays = reservations.Where(r => r.ReservationDate.Day == i).ToList();
                if (todays.Any())
                    todays.ForEach(r => current += Int32.Parse(r.Seats));
                percentages.Add(current * 100 / max);
            }
            
            return percentages;
        }
    }
}
