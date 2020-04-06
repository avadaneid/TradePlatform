namespace TradingPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr_tr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "CompanyIdentifier", c => c.Long());
            AddColumn("dbo.Transactions", "CompanyName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "CompanyName");
            DropColumn("dbo.Transactions", "CompanyIdentifier");
        }
    }
}
