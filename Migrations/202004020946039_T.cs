namespace TradingPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class T : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyFinancialIndicators",
                c => new
                    {
                        Cui = c.Long(nullable: false, identity: true),
                        PriceEarningsRatio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Capitalisation = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceBookValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EarningPerShare = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DividendYield = c.Decimal(precision: 18, scale: 2),
                        Dividend = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Cui);
            
            CreateTable(
                "dbo.Listings",
                c => new
                    {
                        Cui = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Percent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NumberOfShares = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Cui);
            
            AddColumn("dbo.Companies", "DateBeginTransaction", c => c.DateTime(nullable: false));
            AddColumn("dbo.CompanyFinancialDetails", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Individuals", "Debit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Companies", "SharePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.CompanyFinancialDetails", "TotalEquity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.CompanyFinancialDetails", "NetProfit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CompanyFinancialDetails", "NetProfit", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.CompanyFinancialDetails", "TotalEquity", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Companies", "SharePrice", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.Individuals", "Debit");
            DropColumn("dbo.CompanyFinancialDetails", "CreatedOn");
            DropColumn("dbo.Companies", "DateBeginTransaction");
            DropTable("dbo.Listings");
            DropTable("dbo.CompanyFinancialIndicators");
        }
    }
}
