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
            return ctx.Foods.OrderBy(f => f.Type).ThenBy(m => m.Name).ToList();
        }

        public List<EnumItem> GetFoodTypes()
        {
            return Enum.GetValues(typeof(FoodType)).Cast<FoodType>().Select(x => new EnumItem() { Id = (int)x, Description = EnumExtensions.ToString(x) }).OrderBy(x => x.Description).ToList();
        }
    }
}