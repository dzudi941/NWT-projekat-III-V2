namespace jdrive_backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class driver_passenger : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Rides", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Rides", new[] { "ApplicationUser_Id" });
            AddColumn("dbo.AspNetUsers", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Rides", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rides", "ApplicationUser_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.AspNetUsers", "Discriminator");
            CreateIndex("dbo.Rides", "ApplicationUser_Id");
            AddForeignKey("dbo.Rides", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
