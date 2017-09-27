using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Green.Entities;
using Green.Models;

namespace Green.Interfaces
{
    public interface IUserFavoritesQueryService
    {
        List<UserFavorites> GetUsersFavorites();
        List<Restaurant> GetRestaurants();
        List<Image> GetImages();
        List<UserRestaurant> GetUserFavorites(string UserId);
    }
}
