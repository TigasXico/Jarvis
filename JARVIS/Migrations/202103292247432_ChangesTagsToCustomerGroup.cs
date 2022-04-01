namespace Jarvis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesTagsToCustomerGroup : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Tags", newName: "CustomerGroups");
            DropForeignKey("dbo.FiscalEntityTags", "FiscalEntityID", "dbo.FiscalEntities");
            DropForeignKey("dbo.FiscalEntityTags", "TagID", "dbo.Tags");
            DropIndex("dbo.FiscalEntityTags", new[] { "FiscalEntityID" });
            DropIndex("dbo.FiscalEntityTags", new[] { "TagID" });
            AddColumn("dbo.FiscalEntities", "CustomerGroupId", c => c.Int());
            AddColumn("dbo.CustomerGroups", "GroupAbreviation", c => c.String());
            CreateIndex("dbo.FiscalEntities", "CustomerGroupId");
            AddForeignKey("dbo.FiscalEntities", "CustomerGroupId", "dbo.CustomerGroups", "ID");
            DropTable("dbo.FiscalEntityTags");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FiscalEntityTags",
                c => new
                    {
                        FiscalEntityID = c.Int(nullable: false),
                        TagID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.FiscalEntityID, t.TagID });
            
            DropForeignKey("dbo.FiscalEntities", "CustomerGroupId", "dbo.CustomerGroups");
            DropIndex("dbo.FiscalEntities", new[] { "CustomerGroupId" });
            DropColumn("dbo.CustomerGroups", "GroupAbreviation");
            DropColumn("dbo.FiscalEntities", "CustomerGroupId");
            CreateIndex("dbo.FiscalEntityTags", "TagID");
            CreateIndex("dbo.FiscalEntityTags", "FiscalEntityID");
            AddForeignKey("dbo.FiscalEntityTags", "TagID", "dbo.Tags", "ID", cascadeDelete: true);
            AddForeignKey("dbo.FiscalEntityTags", "FiscalEntityID", "dbo.FiscalEntities", "ID", cascadeDelete: true);
            RenameTable(name: "dbo.CustomerGroups", newName: "Tags");
        }
    }
}
