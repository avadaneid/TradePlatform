namespace TradingPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class b : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Companies", "NumberOfTotalShares", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Companies", "NumberOfTotalShares", c => c.Int());
        }
    }
}
