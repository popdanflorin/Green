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
              new Food { Id = Guid.NewGuid().ToString(), Name = "Bread", Type = FoodType.Cereals },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Pasta", Type = FoodType.Cereals },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Wholegrain Pasta", Type = FoodType.Cereals },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Flour", Type = FoodType.Cereals },

              new Food { Id = Guid.NewGuid().ToString(), Name = "Apple", Type = FoodType.Fruits },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Bananas", Type = FoodType.Fruits },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Orange", Type = FoodType.Fruits },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Pineapple", Type = FoodType.Fruits },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Strawberry", Type = FoodType.Fruits },

              new Food { Id = Guid.NewGuid().ToString(), Name = "Pepper", Type = FoodType.Vegetables },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Potatoes", Type = FoodType.Vegetables },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Mushrooms", Type = FoodType.Vegetables },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Tomato", Type = FoodType.Vegetables },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Cabbage", Type = FoodType.Vegetables },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Onions", Type = FoodType.Vegetables },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Carrots", Type = FoodType.Vegetables },

              new Food { Id = Guid.NewGuid().ToString(), Name = "Beer", Type = FoodType.Alcohol },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Wine", Type = FoodType.Alcohol },

              new Food { Id = Guid.NewGuid().ToString(), Name = "Chicken Breast", Type = FoodType.Meats },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Ham", Type = FoodType.Meats },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Bacon", Type = FoodType.Meats },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Pepperoni", Type = FoodType.Meats },
              
              new Food { Id = Guid.NewGuid().ToString(), Name = "Mozzarella", Type = FoodType.Dairy },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Parmesan", Type = FoodType.Dairy },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Mascarpone", Type = FoodType.Dairy },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Cheese cream", Type = FoodType.Dairy },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Milk", Type = FoodType.Dairy },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Eggs", Type = FoodType.Dairy },

              new Food { Id = Guid.NewGuid().ToString(), Name = "Nutella", Type = FoodType.Sweets },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Strawberry jam", Type = FoodType.Sweets },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Cacao Biscuits", Type = FoodType.Sweets },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Plain Biscuits", Type = FoodType.Sweets },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Oreo", Type = FoodType.Sweets },

              new Food { Id = Guid.NewGuid().ToString(), Name = "Basil", Type = FoodType.Spices },
              new Food { Id = Guid.NewGuid().ToString(), Name = "Oregano", Type = FoodType.Spices },

              new Food { Id = Guid.NewGuid().ToString(), Name = "Tomato Sauce", Type = FoodType.SaucesAndToppings },
              new Food { Id = Guid.NewGuid().ToString(), Name = "TChocolate Topping", Type = FoodType.SaucesAndToppings }
            );
            context.SaveChanges();

            /* Add Meals and Ingredients to them */
            string mealId;

            mealId = Guid.NewGuid().ToString();
            context.Meals.AddOrUpdate(new Meal { Id = mealId, Name = "Hawaii Pizza", Description = "...", ImageName = "hawaiiPizza.jpg", Type = MealType.Pizza });
            context.SaveChanges();
            context.MealIngredients.AddOrUpdate(
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Tomato Sauce") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Pineapple") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Bacon") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Mozzarella") == 0).Id }
                );

            mealId = Guid.NewGuid().ToString();
            context.Meals.AddOrUpdate(new Meal { Id = mealId, Name = "Vegetarian Pizza", Description = "...", ImageName = "vegetarianPizza.jpeg", Type = MealType.Pizza });
            context.SaveChanges();
            context.MealIngredients.AddOrUpdate(
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Tomato Sauce") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Mushrooms") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Onions") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Tomato") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Pepper") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Mozzarella") == 0).Id }
                );

            mealId = Guid.NewGuid().ToString();
            context.Meals.AddOrUpdate(new Meal { Id = mealId, Name = "Cheese Spaghetti", ImageName = "cheeseSpaghetti.jpeg", Description = "...", Type = MealType.Pasta });
            context.SaveChanges();
            context.MealIngredients.AddOrUpdate(
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Parmesan") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Mozzarella") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Cheese cream") == 0).Id }
                );

            mealId = Guid.NewGuid().ToString();
            context.Meals.AddOrUpdate(new Meal { Id = mealId, Name = "Meat Lasagna", ImageName = "meatLasagna.jpg", Description = "...", Type = MealType.Pasta });
            context.SaveChanges();
            context.MealIngredients.AddOrUpdate(
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Mozzarella") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Tomato Sauce") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Wholegrain Pasta") == 0).Id }
                );

            mealId = Guid.NewGuid().ToString();
            context.Meals.AddOrUpdate(new Meal { Id = mealId, Name = "Mushrooms Lasagna", ImageName = "mushroomsLasagna.jpg", Description = "...", Type = MealType.Pasta });
            context.SaveChanges();
            context.MealIngredients.AddOrUpdate(
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Mushrooms") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Mozzarella") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Tomato Sauce") == 0).Id }
                );

            mealId = Guid.NewGuid().ToString();
            context.Meals.AddOrUpdate(new Meal { Id = mealId, Name = "Stuffed Mushrooms", ImageName = "stuffedMushrooms.jpg", Description = "...", Type = MealType.VegetarianDishes });
            context.SaveChanges();
            context.MealIngredients.AddOrUpdate(
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Mushrooms") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Mozzarella") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Onions") == 0).Id }
                );

            mealId = Guid.NewGuid().ToString();
            context.Meals.AddOrUpdate(new Meal { Id = mealId, Name = "Oreo Cheesecake", ImageName = "oreoCheesecake.jpg", Description = "...", Type = MealType.Dessert });
            context.SaveChanges();
            context.MealIngredients.AddOrUpdate(
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Oreo") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Cacao Biscuits") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Cheese cream") == 0).Id }
                );

            mealId = Guid.NewGuid().ToString();
            context.Meals.AddOrUpdate(new Meal { Id = mealId, Name = "Nutella Bananas Pancakes", ImageName = "nutellaBananasPancakes.jpg", Description = "...", Type = MealType.Dessert });
            context.SaveChanges();
            context.MealIngredients.AddOrUpdate(
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Nutella") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Bananas") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Milk") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Flour") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Eggs") == 0).Id }
                );

            mealId = Guid.NewGuid().ToString();
            context.Meals.AddOrUpdate(new Meal { Id = mealId, Name = "Strawberry Jam Pancakes", ImageName = "strawberryJamPancakes.jpg", Description = "...", Type = MealType.Dessert });
            context.SaveChanges();
            context.MealIngredients.AddOrUpdate(
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Strawberry jam") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Strawberry") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Milk") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Flour") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Eggs") == 0).Id }
                );

            mealId = Guid.NewGuid().ToString();
            context.Meals.AddOrUpdate(new Meal { Id = mealId, Name = "Tiramisu", ImageName = "tiramisu.jpeg", Description = "...", Type = MealType.Dessert });
            context.SaveChanges();
            context.MealIngredients.AddOrUpdate(
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Plain Biscuits") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Mascarpone") == 0).Id },
                new MealIngredient { Id = Guid.NewGuid().ToString(), MealId = mealId, FoodId = context.Foods.FirstOrDefault(f => f.Name.CompareTo("Eggs") == 0).Id }
                );

            /* Add AppAdmins (Restaurants Managers) */
            if (!context.Roles.Any(r => r.Name == "AppAdmin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "AppAdmin" };

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.Email == "founder@gmail.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "founder@gmail.com", Email = "founder@gmail.com" };

                manager.Create(user, "1Tecknoworker!");
                manager.AddToRole(user.Id, "AppAdmin");
            }

            if (!context.Users.Any(u => u.Email == "founder2@gmail.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "founder2@gmail.com", Email = "founder2@gmail.com" };

                manager.Create(user, "1Tecknoworker!");
                manager.AddToRole(user.Id, "AppAdmin");
            }

            /* Add restaurants */
            context.Restaurants.AddOrUpdate(
              f => f.Name,
                new Restaurant { id = Guid.NewGuid().ToString(), OwnerId = context.Users.FirstOrDefault(u => u.Email.CompareTo("founder@gmail.com") == 0).Id, Name = "Panemar", Address = "Dorobantilor,30,Cluj-Napoca", Type = RestaurantType.Backery, SeatsAvailable = 10, OpeningHour = 10, ClosingHour = 22 },
                new Restaurant { id = Guid.NewGuid().ToString(), OwnerId = context.Users.FirstOrDefault(u => u.Email.CompareTo("founder@gmail.com") == 0).Id, Name = "Samsara", Address = "Str.Roth Stephan Ludwig,nr 5,Cluj-Napoca", Type = RestaurantType.Vegan, SeatsAvailable = 20, OpeningHour = 15, ClosingHour = 20 },
                new Restaurant { id = Guid.NewGuid().ToString(), OwnerId = context.Users.FirstOrDefault(u => u.Email.CompareTo("founder2@gmail.com") == 0).Id, Name = "Tokyo", Address = "Str.Marinescu Gheorghe,nr 5,Cluj-Napoca", Type = RestaurantType.Traditional, SeatsAvailable = 15, OpeningHour = 8, ClosingHour = 21 },
                new Restaurant { id = Guid.NewGuid().ToString(), OwnerId = context.Users.FirstOrDefault(u => u.Email.CompareTo("founder@gmail.com") == 0).Id, Name = "KFC", Address = " Strada Iuliu Maniu,nr 1,Cluj-Napoca", Type = RestaurantType.FastFood, SeatsAvailable = 30, OpeningHour = 11, ClosingHour = 22 },
                new Restaurant { id = Guid.NewGuid().ToString(), OwnerId = context.Users.FirstOrDefault(u => u.Email.CompareTo("founder2@gmail.com") == 0).Id, Name = "Baracca", Address = "Strada Napoca 8A,Cluj-Napoca", Type = RestaurantType.Traditional, SeatsAvailable = 29, OpeningHour = 12, ClosingHour = 23 },
                new Restaurant { id = Guid.NewGuid().ToString(), OwnerId = context.Users.FirstOrDefault(u => u.Email.CompareTo("founder2@gmail.com") == 0).Id, Name = "Nuka Bistro", Address = " Strada Episcop Ioan Bob,9,Cluj-Napoca", Type = RestaurantType.Vegan, SeatsAvailable = 40, OpeningHour = 13, ClosingHour = 22 },
                new Restaurant { id = Guid.NewGuid().ToString(), OwnerId = context.Users.FirstOrDefault(u => u.Email.CompareTo("founder@gmail.com") == 0).Id, Name = "Indigo", Address = "Strada Piezisa,nr 10-12,Cluj-Napoca", Type = RestaurantType.Traditional, SeatsAvailable = 50, OpeningHour = 9, ClosingHour = 23 },
                new Restaurant { id = Guid.NewGuid().ToString(), OwnerId = context.Users.FirstOrDefault(u => u.Email.CompareTo("founder@gmail.com") == 0).Id, Name = "Pralina", Address = "Mihai Viteazul,104,Cluj-Napoca", Type = RestaurantType.Pastry, SeatsAvailable = 12, OpeningHour = 8, ClosingHour = 20 },
                new Restaurant { id = Guid.NewGuid().ToString(), OwnerId = context.Users.FirstOrDefault(u => u.Email.CompareTo("founder2@gmail.com") == 0).Id, Name = "Verde", Address = "George Cosbuc,9,Cluj-Napoca", Type = RestaurantType.Vegetarian, SeatsAvailable = 22, OpeningHour = 7, ClosingHour = 22 }
            );
            context.SaveChanges();

            /* Add images to restaurants */
            context.Images.AddOrUpdate(
                f => f.Name,
                new Image { Id = Guid.NewGuid().ToString(), Name = "kfc1.jpg", isCover = false, RestaurantId = context.Restaurants.FirstOrDefault(f => f.Name.CompareTo("KFC") == 0).id },
                new Image { Id = Guid.NewGuid().ToString(), Name = "kfc2.jpg", isCover = true, RestaurantId = context.Restaurants.FirstOrDefault(f => f.Name.CompareTo("KFC") == 0).id },
                new Image { Id = Guid.NewGuid().ToString(), Name = "nuka1.jpg", isCover = false, RestaurantId = context.Restaurants.FirstOrDefault(f => f.Name.CompareTo("Nuka Bistro") == 0).id },
                new Image { Id = Guid.NewGuid().ToString(), Name = "panemar1.jpg", isCover = false, RestaurantId = context.Restaurants.FirstOrDefault(f => f.Name.CompareTo("Panemar") == 0).id },
                new Image { Id = Guid.NewGuid().ToString(), Name = "pralina1.jpg", isCover = false, RestaurantId = context.Restaurants.FirstOrDefault(f => f.Name.CompareTo("Pralina") == 0).id },
                new Image { Id = Guid.NewGuid().ToString(), Name = "tokyo1.jpg", isCover = false, RestaurantId = context.Restaurants.FirstOrDefault(f => f.Name.CompareTo("Tokyo") == 0).id },
                new Image { Id = Guid.NewGuid().ToString(), Name = "baracca1.jpg", isCover = false, RestaurantId = context.Restaurants.FirstOrDefault(f => f.Name.CompareTo("Baracca") == 0).id },
                new Image { Id = Guid.NewGuid().ToString(), Name = "samsara1.jpg", isCover = false, RestaurantId = context.Restaurants.FirstOrDefault(f => f.Name.CompareTo("Samsara") == 0).id },
                new Image { Id = Guid.NewGuid().ToString(), Name = "verde1.jpg", isCover = false, RestaurantId = context.Restaurants.FirstOrDefault(f => f.Name.CompareTo("Verde") == 0).id },
                new Image { Id = Guid.NewGuid().ToString(), Name = "indigo1.jpg", isCover = false, RestaurantId = context.Restaurants.FirstOrDefault(f => f.Name.CompareTo("Indigo") == 0).id }
            );

            /* Add Normal Users*/
            if (!context.Roles.Any(r => r.Name == "NormalUser"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "NormalUser" };

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.Email == "client@gmail.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "client", Email = "client@gmail.com" };

                manager.Create(user, "1Tecknoworker!");
                manager.AddToRole(user.Id, "NormalUser");
            }
        }
    }
}
