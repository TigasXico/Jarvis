namespace Jarvis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCurrentBalanceToFiscalEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FiscalEntities", "CurrentBalance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FiscalEntities", "CurrentBalance");
        }
    }
}
