namespace TradingPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transaction_logic : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Portofolios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CNP = c.Long(nullable: false),
                        CUI = c.Long(nullable: false),
                        CompanyName = c.String(),
                        Quantity = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CNP = c.Long(nullable: false),
                        CUI = c.Long(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Companies", "SharesOnInitialIPO", c => c.Int());
            AddColumn("dbo.Companies", "Debit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ASKs", "IsIPO", c => c.Boolean(nullable: false));
            DropColumn("dbo.Companies", "SharesOnTheMarket");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "SharesOnTheMarket", c => c.Int(nullable: false));
            DropColumn("dbo.ASKs", "IsIPO");
            DropColumn("dbo.Companies", "Debit");
            DropColumn("dbo.Companies", "SharesOnInitialIPO");
            DropTable("dbo.Transactions");
            DropTable("dbo.Portofolios");
        }
    }
}
