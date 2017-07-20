using Green.Entities;
using Green.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Green.Services
{
    public class CommandService
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();
        public string SaveFood(Food food)
        {
            try
            {
                ctx.Foods.Add(food);
                ctx.SaveChanges();
                return "Item sucessfully saved.";
            }
            catch
            {
                return "An application exception occured when saving.";
            }
        }
    }
}