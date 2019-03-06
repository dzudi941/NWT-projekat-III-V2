using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace jDrive.DataModel.Models
{
    public class JDriveDbContext : IdentityDbContext<ApplicationUser>
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

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Ride>()
        //        .HasRequired(a => a.Passenger);

        //}
    }
}