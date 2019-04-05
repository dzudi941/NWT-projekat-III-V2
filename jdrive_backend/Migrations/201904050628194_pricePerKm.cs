namespace jdrive_backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pricePerKm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PricePerKm", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PricePerKm");
        }
    }
}
