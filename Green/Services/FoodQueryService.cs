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
            var tmp = ctx.Foods.ToList();
            tmp.Sort((e1, e2) => e1.TypeDisplay.CompareTo(e2.TypeDisplay));
            return tmp;
        }

        public List<EnumItem> GetFoodTypes()
        {
            var tmp = Enum.GetValues(typeof(FoodType)).Cast<FoodType>().Select(x => new EnumItem() { Id = (int)x, Description = x.ToString() }).ToList();
            tmp.Sort((e1, e2) => e1.Description.CompareTo(e2.Description));
            return tmp;
        }
    }
}