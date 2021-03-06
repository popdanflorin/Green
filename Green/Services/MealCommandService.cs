﻿using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Green.Entities;
using Green.Models;
using Green.Entities.Enums;
using System.Web.Mvc;
using Green.Services;
using System.Text;
using System.IO;
using Green.Interfaces;

namespace Green.Services
{
    public class MealCommandService : IMealCommandService
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();
        private const string SuccessMessage = "Action sucessfully performed.";
        private const string ErrorMessage = "An application exception occured performing action.";
        private const string ItemNotFoundMessage = "The item was not found.";

        private IMenuCommandService cMenuService;

        public MealCommandService(IMenuCommandService _cMenuService)
        {
            cMenuService = _cMenuService;
        }

        public string SaveMeal(Meal meal, List<MealIngredientDisplay> allIngredients)
        {
            try
            {
                var ingredients = allIngredients.Where(i => i.isSelected == true).Select(i => new Food
                {
                    Id = i.Id,
                    Name = i.Name,
                    Type = i.Type
                }).ToList();

                var oldMeal = ctx.Meals.FirstOrDefault(f => f.Id == meal.Id);
                if (oldMeal == null)
                {
                    meal.Id = Guid.NewGuid().ToString();
                    if (meal.ImageName == null || meal.ImageName.Length == 0)
                        meal.ImageName = "imageNotFound.png";
                    ctx.Meals.Add(meal);
                }
                else
                {
                    oldMeal.Description = meal.Description;
                    oldMeal.Type = meal.Type;
                    oldMeal.ImageName = meal.ImageName == null || meal.ImageName.Length == 0 ? "imageNotFound.png" : meal.ImageName;
                    DeleteAllIngredients(meal.Id);
                }

                if (ingredients != null)
                    SaveIngredients(meal.Id, ingredients);
                ctx.SaveChanges();
                return SuccessMessage;
            }
            catch
            {
                return ErrorMessage;
            }
        }

        public string DeleteMeal(string mealId)
        {
            try
            {
                var meal = ctx.Meals.FirstOrDefault(f => f.Id == mealId);
                if (meal != null)
                {
                    // delete meal ingredients
                    DeleteAllIngredients(mealId);
                    // delete meal from menus
                    cMenuService.DeleteMealFromMenus(mealId);
                    ctx.Meals.Remove(meal);
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

        private void SaveIngredients(string mealId, List<Food> ingredients)
        {
            var pairs = ingredients.Select(i => new MealIngredient(Guid.NewGuid().ToString(), mealId, i.Id)).ToList();
            pairs.ForEach(p => ctx.MealIngredients.Add(p));
        }

        private void DeleteAllIngredients(string mealId)
        {
            var pairs = ctx.MealIngredients.Where(e => e.MealId == mealId).ToList();
            pairs.ForEach(p => ctx.MealIngredients.Remove(p));
        }

        public void SaveImage(string ImageName, string mealId)
        {
            //Image newRecord = new Image();
            //newRecord.Id = Guid.NewGuid().ToString();
            //newRecord.Name = ImageName;

            var meal = ctx.Meals.FirstOrDefault(m => m.Id == mealId);
            if (meal != null)
            {
                meal.ImageName = ImageName;
                ctx.SaveChanges();
            }
            //newRecord.MealId = mealId;
            //ctx.Images.Add(newRecord);
        }

        public string DeleteImage(string mealId)
        {
            //var image = ctx.Images.FirstOrDefault(i => i.MealId == mealId);
            //if (image != null)
            //{
            //    var imageName = image.Name;
            //    ctx.Images.Remove(image);
            //    return imageName;
            //}
            //return null;

            var meal = ctx.Meals.FirstOrDefault(m => m.Id == mealId);
            if (meal != null && meal.ImageName != null)
            {
                var imageName = meal.ImageName;
                meal.ImageName = null;
                ctx.SaveChanges();
                return imageName;
            }
            return null;
        }
    }
}
