namespace jdrive_backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class discountInPercentage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DiscountInPercentage", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DiscountInPercentage");
        }
    }
}
