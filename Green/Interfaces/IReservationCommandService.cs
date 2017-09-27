using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Green.Entities;

namespace Green.Interfaces
{
    public interface IReservationCommandService
    {
        string SaveReservation(Reservation reservation);
        string DeleteReservation(string id);
        string DeleteReservationForRestaurant(string restaurantId);
        bool ValidateReservationSeats(Reservation reservation);
        bool ValidateReservationHour(Reservation reservation);
    }
}
