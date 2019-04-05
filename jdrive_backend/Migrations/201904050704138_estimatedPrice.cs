namespace jdrive_backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class estimatedPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rides", "EstimatedPrice", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rides", "EstimatedPrice");
        }
    }
}
