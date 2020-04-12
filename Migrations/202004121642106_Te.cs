namespace TradingPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Te : DbMigration
    {
        public override void Up()
        {

        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.TransactionReport");
            AlterColumn("dbo.TransactionReport", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.TransactionReport", "Id");
        }
    }
}
