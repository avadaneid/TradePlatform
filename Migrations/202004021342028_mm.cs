namespace TradingPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mm : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ASKs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CUI = c.Long(nullable: true),
                        CNP = c.Long(nullable: true),
                        CompanyName = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BIDs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CUI = c.Long(nullable: true),
                        CNP = c.Long(nullable: true),
                        CompanyName = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BIDs");
            DropTable("dbo.ASKs");
        }
    }
}
