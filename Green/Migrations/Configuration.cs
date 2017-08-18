namespace Green.Migrations
{
    using Green.Entities;
    using Green.Entities.Enums;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Green.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Green.Models.ApplicationDbContext";
        }

        protected override void Seed(Green.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Foods.AddOrUpdate(
              f => f.Name,
              new Food { Id = Guid.NewGuid().ToString(), Name = "Paine", Type = FoodType.Cereals },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Mar", Type = FoodType.Fruits },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Castravete", Type = FoodType.Vegetables },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Bere", Type = FoodType.Alcohol }
            );
            context.Restaurants.AddOrUpdate(
  
                new Restaurant { id = Guid.NewGuid().ToString(), Name = "Panemar", Address = "Dorobantilor,30,Cluj-Napoca", Type = RestaurantType.Backery, MaxPrice = 15,SeatsAvailable=20,Rating=5,MealId=null },
                new Restaurant { id = Guid.NewGuid().ToString(), Name = "Indigo", Address = "Observatorului,21,Cluj-Napoca", Type = RestaurantType.Traditional, MaxPrice = 50,SeatsAvailable = 30, Rating = 2, MealId = null },
                new Restaurant { id = Guid.NewGuid().ToString(), Name = "Pralina", Address = "Mihai Viteazul,104,Cluj-Napoca", Type = RestaurantType.Pastry, MaxPrice = 20, SeatsAvailable = 25, Rating = 3, MealId = null },
                new Restaurant { id = Guid.NewGuid().ToString(), Name = "Verde", Address = "George Cosbuc,9,Cluj-Napoca", Type = RestaurantType.Vegetarian, MaxPrice = 45, SeatsAvailable = 10, Rating = 1, MealId = null }
            );
            if (!context.Roles.Any(r => r.Name == "AppAdmin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "AppAdmin" };

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "founder@gmail.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "founder@gmail.com", Email = "a@b.com" };

                manager.Create(user, "1Tecknoworker!");
                manager.AddToRole(user.Id, "AppAdmin");
            }
        }
    }
}
