namespace TradingPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class affr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanyFinancialDetails", "CUI", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CompanyFinancialDetails", "CUI");
        }
    }
}
