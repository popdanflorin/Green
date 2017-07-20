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

        public void GenerateInitialFoods()
        {
            var f1 = new Food { Id = Guid.NewGuid().ToString(), Name = "Paine", Type = FoodType.Cereals };
            ctx.Foods.Add(f1);
            var f2 = new Food { Id = Guid.NewGuid().ToString(), Name = "Mar", Type = FoodType.Fruits };
            ctx.Foods.Add(f2);
            var f3 = new Food { Id = Guid.NewGuid().ToString(), Name = "Castravete", Type = FoodType.Vegetables };
            ctx.Foods.Add(f3);
            var f4 = new Food { Id = Guid.NewGuid().ToString(), Name = "Bere", Type = FoodType.Alcohol };
            ctx.Foods.Add(f4);
            ctx.SaveChanges();
        }
        public List<Food> GetFoods()
        {
            return ctx.Foods.ToList();
        }
    }
}