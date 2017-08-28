using Green.Entities;
using Green.Entities.Enums;
using Green.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Green.Services
{
    public class FoodQueryService
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();

        public List<Food> GetFoods()
        {
            return ctx.Foods.ToList();
        }

        public List<EnumItem> GetFoodTypes()
        {
            return Enum.GetValues(typeof(FoodType)).Cast<FoodType>().Select(x => new EnumItem() { Id = (int)x, Description = x.ToString() }).ToList();
        }

        public String GetNamesById(List<String> idList)
        {
            try
            {
                var foods = ctx.Foods.Where(f => idList.FirstOrDefault(id => id == f.Id) != null).Select(f => f.Name).ToList();
                foods.Sort();
                String nameList = "";
                foods.ForEach(f => nameList += f + ", ");
                if (nameList.Length >= 2)
                    return nameList.Substring(0, nameList.Length - 2);
                return nameList;
            }
            catch
            {
                return null;
            }
        }
    }
}