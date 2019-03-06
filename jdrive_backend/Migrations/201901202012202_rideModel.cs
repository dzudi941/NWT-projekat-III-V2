namespace jdrive_backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rideModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rides",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartAddress = c.String(),
                        FinishAddress = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        Driver_Id = c.String(maxLength: 128),
                        Passenger_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Driver_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Passenger_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Driver_Id)
                .Index(t => t.Passenger_Id);
            
            AddColumn("dbo.AspNetUsers", "Longitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AspNetUsers", "Latitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rides", "Passenger_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Rides", "Driver_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Rides", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Rides", new[] { "Passenger_Id" });
            DropIndex("dbo.Rides", new[] { "Driver_Id" });
            DropIndex("dbo.Rides", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.AspNetUsers", "Latitude");
            DropColumn("dbo.AspNetUsers", "Longitude");
            DropTable("dbo.Rides");
        }
    }
}
