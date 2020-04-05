namespace TradingPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "SellTo", c => c.Long(nullable: false));
            AddColumn("dbo.Transactions", "BuyFrom", c => c.Long(nullable: false));
            AddColumn("dbo.Transactions", "FromIPO", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Transactions", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Transactions", "CNP");
            DropColumn("dbo.Transactions", "CUI");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transactions", "CUI", c => c.Long(nullable: false));
            AddColumn("dbo.Transactions", "CNP", c => c.Long(nullable: false));
            AlterColumn("dbo.Transactions", "Price", c => c.Int(nullable: false));
            DropColumn("dbo.Transactions", "FromIPO");
            DropColumn("dbo.Transactions", "BuyFrom");
            DropColumn("dbo.Transactions", "SellTo");
        }
    }
}
