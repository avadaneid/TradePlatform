namespace TradingPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class share : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "NumberOfTotalShares", c => c.Int());
            AddColumn("dbo.Companies", "MarketSharePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Companies", "SharesCount");
            DropColumn("dbo.Companies", "SharePrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "SharePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Companies", "SharesCount", c => c.Int());
            DropColumn("dbo.Companies", "MarketSharePrice");
            DropColumn("dbo.Companies", "NumberOfTotalShares");
        }
    }
}
