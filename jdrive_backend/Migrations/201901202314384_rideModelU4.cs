namespace jdrive_backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rideModelU4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Rides", "StartLongitude", c => c.Double(nullable: false));
            AlterColumn("dbo.Rides", "StartLatitude", c => c.Double(nullable: false));
            AlterColumn("dbo.Rides", "FinishLongitude", c => c.Double(nullable: false));
            AlterColumn("dbo.Rides", "FinishLatitude", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Rides", "FinishLatitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Rides", "FinishLongitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Rides", "StartLatitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Rides", "StartLongitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
