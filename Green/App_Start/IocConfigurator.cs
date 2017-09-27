using Green.Infrastructure;
using Green.Services;
using Green.Interfaces;
using Microsoft.Practices.Unity;
using System.Web.Mvc;

namespace Green.App_Start
{
    public static class IocConfigurator
    {
        public static void ConfigureIocUnityContainer()
        {
            IUnityContainer container = new UnityContainer();

            registerServices(container);

            DependencyResolver.SetResolver(new GreenDependencyResolver(container));
        }

        private static void registerServices(IUnityContainer container)
        {
            container.RegisterType<IFoodCommandService, FoodCommandService>();
            container.RegisterType<IFoodQueryService, FoodQueryService>();
            container.RegisterType<IMealCommandService, MealCommandService>();
            container.RegisterType<IMealQueryService, MealQueryService>();
            container.RegisterType<IRatingCommandService, RatingCommandService>();
            container.RegisterType<IRatingQueryService, RatingQueryService>();
            container.RegisterType<IUserFavoritesCommandService, UserFavoritesCommandService>();
            container.RegisterType<IUserFavoritesQueryService, UserFavoritesQueryService>();
            container.RegisterType<IReservationCommandService, ReservationCommandService>();
            container.RegisterType<IReservationQueryService, ReservationQueryService>();
            container.RegisterType<IMenuCommandService, MenuCommandService>();
            container.RegisterType<IMenuQueryService, MenuQueryService>();
            container.RegisterType<IRestaurantCommandService, RestaurantCommandService>();
            container.RegisterType<IRestaurantQueryService, RestaurantQueryService>();
        }
    }
}