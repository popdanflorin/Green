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
              new Food { Id = Guid.NewGuid().ToString(), Name = "Banana", Type = FoodType.Fruits },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Portocala", Type = FoodType.Fruits },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Castravete", Type = FoodType.Vegetables },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Cartof", Type = FoodType.Vegetables },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Rosie", Type = FoodType.Vegetables },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Varza", Type = FoodType.Vegetables },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Salata", Type = FoodType.Vegetables },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Ciuperci", Type = FoodType.Vegetables },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Ardei iute", Type = FoodType.Vegetables },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Bere", Type = FoodType.Alcohol },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Vin", Type = FoodType.Alcohol },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Cod", Type = FoodType.Meats },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Pastrav", Type = FoodType.Meats },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Piept de pui", Type = FoodType.Meats },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Sunca de pui", Type = FoodType.Meats },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Pepperoni", Type = FoodType.Meats },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Telemea", Type = FoodType.Milk },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Mozzarela", Type = FoodType.Milk },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Nutella", Type = FoodType.Sweets },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Gem caise", Type = FoodType.Sweets },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Gem capsuni", Type = FoodType.Sweets }
            );

            context.Restaurants.AddOrUpdate(
              f => f.Name,
                new Restaurant { id = Guid.NewGuid().ToString(), Name = "Panemar", Address = "Dorobantilor,30,Cluj-Napoca", Type = RestaurantType.Backery, SeatsAvailable = 10,OpeningHour=10,ClosingHour = 22 },
                new Restaurant { id = Guid.NewGuid().ToString(), Name = "Samsara", Address = "Str.Roth Stephan Ludwig,nr 5,Cluj-Napoca", Type = RestaurantType.Vegan, SeatsAvailable = 20, OpeningHour = 15,ClosingHour = 20 },
                new Restaurant { id = Guid.NewGuid().ToString(), Name = "Tokyo", Address = "Str.Marinescu Gheorghe,nr 5,Cluj-Napoca", Type = RestaurantType.Traditional, SeatsAvailable = 15, OpeningHour = 8,ClosingHour = 21 },
                new Restaurant { id = Guid.NewGuid().ToString(), Name = "KFC", Address = " Strada Iuliu Maniu,nr 1,Cluj-Napoca", Type = RestaurantType.FastFood, SeatsAvailable = 30, OpeningHour = 11,ClosingHour = 22 },
                new Restaurant { id = Guid.NewGuid().ToString(), Name = "Baracca", Address = "Strada Napoca 8A,Cluj-Napoca", Type = RestaurantType.Traditional, SeatsAvailable = 29, OpeningHour = 12,ClosingHour = 23 },
                new Restaurant { id = Guid.NewGuid().ToString(), Name = "Nuka Bistro", Address = " Strada Episcop Ioan Bob,9,Cluj-Napoca", Type = RestaurantType.Vegan, SeatsAvailable = 40, OpeningHour = 13,ClosingHour = 22 },
                new Restaurant { id = Guid.NewGuid().ToString(), Name = "Indigo", Address = "Strada Piezisa,nr 10-12,Cluj-Napoca", Type = RestaurantType.Traditional, SeatsAvailable = 50, OpeningHour = 9,ClosingHour = 23 },
                new Restaurant { id = Guid.NewGuid().ToString(), Name = "Pralina", Address = "Mihai Viteazul,104,Cluj-Napoca", Type = RestaurantType.Pastry, SeatsAvailable = 12, OpeningHour = 8,ClosingHour = 20 },
                new Restaurant { id = Guid.NewGuid().ToString(), Name = "Verde", Address = "George Cosbuc,9,Cluj-Napoca", Type = RestaurantType.Vegetarian, SeatsAvailable = 22, OpeningHour = 7,ClosingHour = 22 }
            );
          /*  context.Images.AddOrUpdate(
                f => f.Name,
                new Image { Id = Guid.NewGuid().ToString(), Name = "kfc1.jpg", isCover = false, RestaurantId = "0ada21ab-b934-4a74-b890-965fc8d4c5a5" },
                new Image { Id = Guid.NewGuid().ToString(), Name = "kfc2.jpg", isCover = true, RestaurantId = "0ada21ab-b934-4a74-b890-965fc8d4c5a" },
                new Image { Id = Guid.NewGuid().ToString(), Name = "nuka1.jpg", isCover = false, RestaurantId = "267c7a8e-a935-455f-80f8-e75d37a34576" },
                new Image { Id = Guid.NewGuid().ToString(), Name = "panemar1.jpg", isCover = false, RestaurantId = "270684d8-0467-4911-be1d-4bcacf4d0102" },
                new Image { Id = Guid.NewGuid().ToString(), Name = "pralina1.jpg", isCover = false, RestaurantId = "6995bc5a-750a-47a1-b8fc-039bdcdc3bba" },
                new Image { Id = Guid.NewGuid().ToString(), Name = "tokyo1.jpg", isCover = false, RestaurantId = "a9d72ed8-36e9-4be5-a126-0a7f13172858" },
                new Image { Id = Guid.NewGuid().ToString(), Name = "baracca1.jpg", isCover = false, RestaurantId = "bd05a7fc-bc83-403b-9fba-421f41cb3942" },
                new Image { Id = Guid.NewGuid().ToString(), Name = "samsara1.jpg", isCover = false, RestaurantId = "e7b9dc2d-9293-4edb-bd7a-785f83369525" },
                new Image { Id = Guid.NewGuid().ToString(), Name = "verde1.jpg", isCover = false, RestaurantId = "f143c4f5-e61a-4d48-b6b6-3f08987f2981" },
                new Image { Id = Guid.NewGuid().ToString(), Name = "indigo1.jpg", isCover = false, RestaurantId = "fe4c7844-0a24-4419-bfd5-f8c866a05408" }
            );*/
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
                var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "founder@gmail.com", Email = "a@b.com" };

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
                var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "client@gmail.com", Email = "aa@b.com" };

                manager.Create(user, "1Tecknoworker!");
                manager.AddToRole(user.Id, "NormalUser");
            }
        }
    }
}
