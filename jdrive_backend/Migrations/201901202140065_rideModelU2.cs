namespace jdrive_backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rideModelU2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Longitude", c => c.Double(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Latitude", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Latitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.AspNetUsers", "Longitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
