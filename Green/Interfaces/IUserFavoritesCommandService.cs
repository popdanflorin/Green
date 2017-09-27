using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Green.Entities;

namespace Green.Interfaces
{
    public interface IUserFavoritesCommandService
    {
        string SaveFavorite(UserFavorites userFavorite);
        string DeleteFavorite(string Id);
        string DeleteFavoritesForRestaurant(string restaurantId);
    }
}
