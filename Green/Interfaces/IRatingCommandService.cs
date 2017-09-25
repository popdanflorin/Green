using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Green.Entities;

namespace Green.Services
{
    public interface IRatingCommandService
    {
        string SaveRating(Rating rating);
        string DeleteRating(string id);
        string DeleteRatingsForRestaurant(string restaurantId);
    }
}
