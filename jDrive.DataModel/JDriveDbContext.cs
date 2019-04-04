using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace jDrive.DataModel.Models
{
    public interface IJDriveDbContext
    {
        DbSet<Ride> Rides { get; set; }
        DbSet<Driver> Drivers { get; set; }
        DbSet<Passenger> Passengers { get; set; }
        int SaveChanges();
        void Dispose();
        DbSet<T> Set<T>() where T : class;
        DbEntityEntry<T> Entry<T>(T entity) where T : class;
    }

    public class JDriveDbContext : IdentityDbContext<ApplicationUser>, IJDriveDbContext
    {
        public DbSet<Ride> Rides { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Passenger> Passengers { get; set; }

        public JDriveDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static JDriveDbContext Create()
        {

            return new JDriveDbContext();
        }
    }
}