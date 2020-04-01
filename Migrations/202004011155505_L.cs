namespace TradingPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class L : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Companies", "IsListed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Companies", "IsListed", c => c.Boolean());
        }
    }
}
