using Green.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Green.Interfaces
{
    public interface IRestaurantCommandService
    {
        string SaveRestaurant(Restaurant restaurant);
        string SetCoverImage(string restaurantId, string imageId);
        string DeleteRestaurant(string restaurantId);
        List<String> DeleteImages(string restaurantId);
        String DeleteImage(string imageId);
    }
}
