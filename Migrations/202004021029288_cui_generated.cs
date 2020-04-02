namespace TradingPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cui_generated : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.CompanyFinancialIndicators");
            AlterColumn("dbo.CompanyFinancialIndicators", "Cui", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.CompanyFinancialIndicators", "Cui");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.CompanyFinancialIndicators");
            AlterColumn("dbo.CompanyFinancialIndicators", "Cui", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.CompanyFinancialIndicators", "Cui");
        }
    }
}
