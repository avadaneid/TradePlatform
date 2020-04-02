namespace TradingPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class l : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Companies", "DateBeginTransaction", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Companies", "DateBeginTransaction", c => c.DateTime(nullable: false));
        }
    }
}
