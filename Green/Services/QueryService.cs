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

        public List<object> GetFoodTypes()
        {
            return Enum.GetValues(typeof(FoodType)).Cast<FoodType>().Select(x => new { Id = x, Description = x.ToString() }).ToList<object>();
        }
    }
}