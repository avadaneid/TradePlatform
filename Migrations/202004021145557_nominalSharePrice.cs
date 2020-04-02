namespace TradingPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nominalSharePrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "NominalSharePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "NominalSharePrice");
        }
    }
}
