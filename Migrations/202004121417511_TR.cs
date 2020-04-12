namespace TradingPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TR : DbMigration
    {
        public override void Up()
        {      
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TransactionReport");
        }
    }
}
