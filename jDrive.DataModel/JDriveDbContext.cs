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
            //Set<>
            //Entry
        }

        public static JDriveDbContext Create()
        {

            return new JDriveDbContext();
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Ride>()
        //        .HasRequired(a => a.Passenger);

        //}
    }

    //public interface IDbContextHolder
    //{
    //    JDriveDbContext JDriveDbContext { get; }
    //}

    //public class DbContextHolder : IDbContextHolder
    //{
    //    public JDriveDbContext JDriveDbContext
    //    {
    //        get
    //        {
    //            return Test.Instance.JDriveDbContext;
    //        }
    //    }
    //}

    //public class Test
    //{
    //    private static Test _test;
    //    public static Test Instance
    //    {
    //        get
    //        {
    //            return _test ?? (_test = new Test());
    //        }
    //    }
    //    private JDriveDbContext _jDriveDbContext;
    //    public JDriveDbContext JDriveDbContext
    //    {
    //        get
    //        {
    //            return _jDriveDbContext ?? (_jDriveDbContext = new JDriveDbContext());
    //        }
    //    }
    //}
}