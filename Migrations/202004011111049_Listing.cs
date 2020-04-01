namespace TradingPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Listing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "IsListed", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "IsListed");
        }
    }
}
