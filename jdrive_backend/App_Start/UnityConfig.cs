using jDrive.DataModel.Models;
using jDrive.Services.Services;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;

namespace jdrive_backend
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IJDriveDbContext, JDriveDbContext>(new PerResolveLifetimeManager());
            //container.RegisterInstance(typeof(IJDriveDbContext), new JDriveDbContext());
            //container.RegisterType<IDbContextHolder, DbContextHolder>();
            container.RegisterType<IRepository<Ride>, Repository<Ride>>();
            container.RegisterType<IRepository<Driver>, Repository<Driver>>();
            container.RegisterType<IRepository<Passenger>, Repository<Passenger>>();

            container.RegisterType<IRideService, RideService>();
            container.RegisterType<IDriverService, DriverService>();
            container.RegisterType<IPassengerService, PassengerService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}