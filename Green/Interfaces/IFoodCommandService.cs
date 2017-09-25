using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Green.Entities;

namespace Green.Services
{
    public interface IFoodCommandService
    {
        string SaveFood(Food food);
        string DeleteFood(string foodId);
        void DeleteFromMeals(string foodId);
    }
}
