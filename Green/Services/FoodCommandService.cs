using Green.Entities;
using Green.Models;
using System;
using System.Linq;

namespace Green.Services
{
    public class FoodCommandService
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();
        private const string SuccessMessage = "Action sucessfully performed.";
        private const string ErrorMessage = "An application exception occured performing action.";
        private const string ItemNotFoundMessage = "The item was not found.";
        public string SaveFood(Food food)
        {
            try
            {
                var oldFood = ctx.Foods.FirstOrDefault(f => f.Id == food.Id);
                if (oldFood == null)
                {
                    food.Id = Guid.NewGuid().ToString();
                    ctx.Foods.Add(food);
                }
                else
                {
                    oldFood.Name = food.Name;
                    oldFood.Type = food.Type;
                }

                ctx.SaveChanges();
                return SuccessMessage;
            }
            catch
            {
                return ErrorMessage;
            }
        }

        public string DeleteFood(string id)
        {
            try
            {
                var food = ctx.Foods.FirstOrDefault(f => f.Id == id);
                if (food != null)
                {
                    ctx.Foods.Remove(food);
                    ctx.SaveChanges();
                    return SuccessMessage;
                }
                return ItemNotFoundMessage;
            }
            catch (Exception)
            {
                return ErrorMessage;
            }
        }
    }
}