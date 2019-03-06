namespace jdrive_backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rating : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rides", "DriverRating", c => c.Int(nullable: false));
            AddColumn("dbo.Rides", "PassengerRating", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rides", "PassengerRating");
            DropColumn("dbo.Rides", "DriverRating");
        }
    }
}
