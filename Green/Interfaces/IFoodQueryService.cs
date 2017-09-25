using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Green.Entities;
using Green.Entities.Enums;

namespace Green.Services
{
    public interface IFoodQueryService
    {
        List<Food> GetFoods();
        List<EnumItem> GetFoodTypes();
    }
}
