namespace TradingPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class utpp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CompanyName = c.String(),
                        CNP = c.Long(nullable: false),
                        Name = c.String(),
                        Surname = c.String(),
                        PhoneNumber = c.Long(nullable: false),
                        Address = c.String(),
                        UserName = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        AccountType = c.Int(nullable: false),
                        CUI = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CUI = c.Long(nullable: false),
                        UserName = c.String(),
                        CompanyName = c.String(),
                        Simbol = c.String(),
                        SharesCount = c.Int(),
                        SharePrice = c.Decimal(precision: 18, scale: 2),
                        Account_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.CUI)
                .ForeignKey("dbo.Accounts", t => t.Account_Id)
                .Index(t => t.Account_Id);
            
            CreateTable(
                "dbo.CompanyFinancialDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Year = c.Decimal(precision: 18, scale: 2),
                        Quarter = c.Decimal(precision: 18, scale: 2),
                        TotalTangibleAssets = c.Decimal(precision: 18, scale: 2),
                        Shares = c.Decimal(precision: 18, scale: 2),
                        TotalCurrentAssets = c.Decimal(precision: 18, scale: 2),
                        Inventories = c.Decimal(precision: 18, scale: 2),
                        Receivables = c.Decimal(precision: 18, scale: 2),
                        Cash = c.Decimal(precision: 18, scale: 2),
                        ShortTermInvestments = c.Decimal(precision: 18, scale: 2),
                        Prepayments = c.Decimal(precision: 18, scale: 2),
                        TotalOneYearDebts = c.Decimal(precision: 18, scale: 2),
                        FinancialOneYearDebts = c.Decimal(precision: 18, scale: 2),
                        CommercialOneYearDebts = c.Decimal(precision: 18, scale: 2),
                        ReceivablesCurrentDebts = c.Decimal(precision: 18, scale: 2),
                        NetAssets = c.Decimal(precision: 18, scale: 2),
                        LongTermDebts = c.Decimal(precision: 18, scale: 2),
                        FinancialLongTerDebts = c.Decimal(precision: 18, scale: 2),
                        RevenueInAdvance = c.Decimal(precision: 18, scale: 2),
                        SubscribedCapital = c.Decimal(precision: 18, scale: 2),
                        TotalEquity = c.Decimal(precision: 18, scale: 2),
                        TotalDebts = c.Decimal(precision: 18, scale: 2),
                        NetTurnover = c.Decimal(precision: 18, scale: 2),
                        TotalOperatingIncome = c.Decimal(precision: 18, scale: 2),
                        ValueAdjustments = c.Decimal(precision: 18, scale: 2),
                        TotalOperatingExpenses = c.Decimal(precision: 18, scale: 2),
                        NetOperatingIncome = c.Decimal(precision: 18, scale: 2),
                        SharesIncome = c.Decimal(precision: 18, scale: 2),
                        InterestIncome = c.Decimal(precision: 18, scale: 2),
                        TotalFinancialRevenues = c.Decimal(precision: 18, scale: 2),
                        InterestExpenses = c.Decimal(precision: 18, scale: 2),
                        TotalFinanciarExpenses = c.Decimal(precision: 18, scale: 2),
                        FinancialResult = c.Decimal(precision: 18, scale: 2),
                        TotalRevenues = c.Decimal(precision: 18, scale: 2),
                        TotalExpenses = c.Decimal(precision: 18, scale: 2),
                        GrossProfit = c.Decimal(precision: 18, scale: 2),
                        NetProfit = c.Decimal(precision: 18, scale: 2),
                        NumberOfEmployees = c.Decimal(precision: 18, scale: 2),
                        Company_CUI = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.Company_CUI)
                .Index(t => t.Company_CUI);
            
            CreateTable(
                "dbo.Individuals",
                c => new
                    {
                        CNP = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        PhoneNumber = c.Long(nullable: false),
                        Address = c.String(),
                        UserName = c.String(),
                        Account_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.CNP)
                .ForeignKey("dbo.Accounts", t => t.Account_Id)
                .Index(t => t.Account_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Individuals", "Account_Id", "dbo.Accounts");
            DropForeignKey("dbo.Companies", "Account_Id", "dbo.Accounts");
            DropForeignKey("dbo.CompanyFinancialDetails", "Company_CUI", "dbo.Companies");
            DropIndex("dbo.Individuals", new[] { "Account_Id" });
            DropIndex("dbo.CompanyFinancialDetails", new[] { "Company_CUI" });
            DropIndex("dbo.Companies", new[] { "Account_Id" });
            DropTable("dbo.Individuals");
            DropTable("dbo.CompanyFinancialDetails");
            DropTable("dbo.Companies");
            DropTable("dbo.Accounts");
        }
    }
}
