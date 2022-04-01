namespace Jarvis.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UpdatedCustomerGRoupWithInternalIdCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerGroups", "CurrentIdCount", c => c.Int(nullable: false));
            DropColumn("dbo.CustomerGroups", "GroupAbreviation");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CustomerGroups", "GroupAbreviation", c => c.String());
            DropColumn("dbo.CustomerGroups", "CurrentIdCount");
        }
    }
}
