namespace TradingPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyName : DbMigration
    {
        public override void Up()
        {
           
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransactionReport", "CompanyName");
        }
    }
}
