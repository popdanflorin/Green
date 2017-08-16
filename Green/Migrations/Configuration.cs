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
        }
    }
}
