using jDrive.DomainModel.Models;
using jDrive.DomainModel;
using jDrive.Repositories.Repositories;
using jDrive.Services.Services;
using System.Web;
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
            container.RegisterType<IJDriveDbContext, JDriveDbContext>(new PerRequestLifetimeManager());
            container.RegisterType<IRepository<Ride>, Repository<Ride>>();
            container.RegisterType<IRepository<Driver>, Repository<Driver>>();
            container.RegisterType<IRepository<Passenger>, Repository<Passenger>>();

            container.RegisterType<IRideService, RideService>();
            container.RegisterType<IDriverService, DriverService>();
            container.RegisterType<IPassengerService, PassengerService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }

    public class PerRequestLifetimeManager : LifetimeManager
    {
        private readonly object key = new object();

        public override object GetValue()
        {
            if (HttpContext.Current != null &&
                HttpContext.Current.Items.Contains(key))
                return HttpContext.Current.Items[key];
            else
                return null;
        }

        public override void RemoveValue()
        {
            if (HttpContext.Current != null)
                HttpContext.Current.Items.Remove(key);
        }

        public override void SetValue(object newValue)
        {
            if (HttpContext.Current != null)
                HttpContext.Current.Items[key] = newValue;
        }
    }
}