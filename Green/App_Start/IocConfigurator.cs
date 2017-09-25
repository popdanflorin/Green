using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Green.Services;
using Microsoft.Practices.Unity;
using System.Web.Mvc;
using Green.Infrastructure;

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
        }
    }
}