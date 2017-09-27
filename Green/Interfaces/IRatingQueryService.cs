using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Green.Entities;

namespace Green.Services
{
    public interface IRatingQueryService
    {
        List<Rating> GetRatings();
        int GetUserRating(string UserId, string RestaurantId);
        int GetTotalRating(string RestaurantId);
    }
}
