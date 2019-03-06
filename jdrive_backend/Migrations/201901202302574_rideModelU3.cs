namespace jdrive_backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rideModelU3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rides", "RequestStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rides", "RequestStatus");
        }
    }
}
