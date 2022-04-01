namespace Jarvis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFamilyAggregatesToDatabase : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.EntitiesImiPayments", newName: "ImiPayments");
            CreateTable(
                "dbo.Aggregates",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AggregateName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Clients", "Aggregate_ID", c => c.Int());
            CreateIndex("dbo.Clients", "Aggregate_ID");
            AddForeignKey("dbo.Clients", "Aggregate_ID", "dbo.Aggregates", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clients", "Aggregate_ID", "dbo.Aggregates");
            DropIndex("dbo.Clients", new[] { "Aggregate_ID" });
            DropColumn("dbo.Clients", "Aggregate_ID");
            DropTable("dbo.Aggregates");
            RenameTable(name: "dbo.ImiPayments", newName: "EntitiesImiPayments");
        }
    }
}
