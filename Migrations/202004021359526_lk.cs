namespace TradingPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lk : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Individuals");
            AlterColumn("dbo.Individuals", "CNP", c => c.Long(nullable: false, identity: false));
            AddPrimaryKey("dbo.Individuals", "CNP");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Individuals");
            AlterColumn("dbo.Individuals", "CNP", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Individuals", "CNP");
        }
    }
}
