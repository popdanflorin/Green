using Green.Entities;
using Green.Entities.Enums;
using Green.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Green.Services
{
    public class QueryService
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
    }
}