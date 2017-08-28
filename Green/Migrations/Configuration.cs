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
              f=>f.Name,
                new Restaurant { id = Guid.NewGuid().ToString(), Name = "Panemar", Address = "Dorobantilor,30,Cluj-Napoca", Type = RestaurantType.Backery, SeatsAvailable = 10, ClosingHour = 22 },
                new Restaurant { id = Guid.NewGuid().ToString(), Name = "Indigo", Address = "Observatorului,21,Cluj-Napoca", Type = RestaurantType.Traditional, SeatsAvailable = 10, ClosingHour = 22 },
                new Restaurant { id = Guid.NewGuid().ToString(), Name = "Pralina", Address = "Mihai Viteazul,104,Cluj-Napoca", Type = RestaurantType.Pastry, SeatsAvailable = 10, ClosingHour = 22 },
                new Restaurant { id = Guid.NewGuid().ToString(), Name = "Verde", Address = "George Cosbuc,9,Cluj-Napoca", Type = RestaurantType.Vegetarian, SeatsAvailable = 10, ClosingHour = 22 }
            );
            if (!context.Roles.Any(r => r.Name == "AppAdmin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "AppAdmin" };

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "founder"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "founder@gmail.com", Email = "a@b.com" };

                manager.Create(user, "1Tecknoworker!");
                manager.AddToRole(user.Id, "AppAdmin");
            }


            if (!context.Roles.Any(r => r.Name == "NormalUser"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "NormalUser" };

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "client"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "client@gmail.com", Email = "aa@b.com" };

                manager.Create(user, "1Tecknoworker!");
                manager.AddToRole(user.Id, "NormalUser");
            }
        }
    }
}
