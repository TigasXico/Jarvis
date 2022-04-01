namespace Jarvis.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UpdatedCustomerGroupIdCountingMechanism : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FiscalEntities", "IdOnCustomerGroup", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FiscalEntities", "IdOnCustomerGroup");
        }
    }
}
