namespace Green.Migrations
{
    using Green.Entities;
    using Green.Entities.Enums;
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

            context.Reservations.AddOrUpdate(
                new Reservation { Id = Reservation.IdCounter, ClientName = "Popescu", ReservationDate = new DateTime(2017, 9, 12, 20, 30, 0), Seats = 4 },
                new Reservation { Id = Reservation.IdCounter, ClientName = "Ionscu", ReservationDate = new DateTime(2018, 1, 25, 15, 0, 0), Seats = 1 },
                new Reservation { Id = Reservation.IdCounter, ClientName = "Grigorescu", ReservationDate = new DateTime(2017, 10, 7, 21, 30, 0), Seats = 3 }
            );
            context.Restaurants.AddOrUpdate(
  
                new Restaurant { id = Guid.NewGuid().ToString(), Name = "Panemar", Address = "Dorobantilor,30,Cluj-Napoca", Type = RestaurantType.Backery, MaxPrice = 15 },
                new Restaurant { id = Guid.NewGuid().ToString(), Name = "Indigo", Address = "Observatorului,21,Cluj-Napoca", Type = RestaurantType.Traditional, MaxPrice = 50 },
                new Restaurant { id = Guid.NewGuid().ToString(), Name = "Pralina", Address = "Mihai Viteazul,104,Cluj-Napoca", Type = RestaurantType.Pastry, MaxPrice = 20 },
                new Restaurant { id = Guid.NewGuid().ToString(), Name = "Verde", Address = "George Cosbuc,9,Cluj-Napoca", Type = RestaurantType.Vegetarian, MaxPrice = 45 }
            );
        }
    }
}
