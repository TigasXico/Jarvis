namespace Jarvis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFiscalEntityStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FiscalEntities", "CurrentStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FiscalEntities", "CurrentStatus");
        }
    }
}
