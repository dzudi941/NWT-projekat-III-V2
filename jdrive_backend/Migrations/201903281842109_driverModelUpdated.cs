namespace jdrive_backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class driverModelUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "RideDiscountNumber", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "RideDiscountNumber");
        }
    }
}
