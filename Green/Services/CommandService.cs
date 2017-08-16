using Green.Entities;
using Green.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Green.Services
{
    public class CommandService
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

        public string SaveMeal(Meal meal)
        {
            try
            {
                var oldMeal = ctx.Meals.FirstOrDefault(f => f.Id == meal.Id);
                if (oldMeal == null)
                {
                    meal.Id = Guid.NewGuid().ToString();
                    meal.PlannedTime = DateTime.Now;
                    meal.ActualTime = DateTime.Now;
                    ctx.Meals.Add(meal);
                }
                else
                {
                    oldMeal.Details = meal.Details;
                    oldMeal.PreparationDetails = meal.PreparationDetails;
                    oldMeal.Type = meal.Type;
                    oldMeal.Status = meal.Status;
                    oldMeal.Rating = meal.Rating;
                }

                ctx.SaveChanges();
                return SuccessMessage;
            }
            catch
            {
                return ErrorMessage;
            }
        }

        public string DeleteMeal(string id)
        {
            try
            {
                var meal = ctx.Meals.FirstOrDefault(f => f.Id == id);
                if (meal != null)
                {
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

        public string SaveReservation(Reservation reservation)
        {
            try
            {
                var oldReservation = ctx.Reservations.First(r => r.Id == reservation.Id);
                if (oldReservation == null)
                {
                    reservation.Id = Reservation.IdCounter;
                    Reservation.IdCounter += 1;
                    ctx.Reservations.Add(reservation);
                }
                else
                {
                    oldReservation.ClientName = reservation.ClientName;
                    oldReservation.ReservationDate = reservation.ReservationDate;
                    oldReservation.Seats = reservation.Seats;
                }

                ctx.SaveChanges();
                return SuccessMessage;
            }
            catch (Exception)
            {
                return ErrorMessage;
            }
        }
    }
}