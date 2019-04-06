using jDrive.DomainModel;
using System.Data.Entity.Migrations;


namespace jdrive_backend.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<JDriveDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "jdrive_backend.Models.ApplicationDbContext";
        }

        protected override void Seed(JDriveDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
