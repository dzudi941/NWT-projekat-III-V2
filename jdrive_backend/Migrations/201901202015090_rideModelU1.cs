namespace jdrive_backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rideModelU1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rides", "StartLongitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Rides", "StartLatitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Rides", "FinishLongitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Rides", "FinishLatitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Rides", "StartAddress");
            DropColumn("dbo.Rides", "FinishAddress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rides", "FinishAddress", c => c.String());
            AddColumn("dbo.Rides", "StartAddress", c => c.String());
            DropColumn("dbo.Rides", "FinishLatitude");
            DropColumn("dbo.Rides", "FinishLongitude");
            DropColumn("dbo.Rides", "StartLatitude");
            DropColumn("dbo.Rides", "StartLongitude");
        }
    }
}
