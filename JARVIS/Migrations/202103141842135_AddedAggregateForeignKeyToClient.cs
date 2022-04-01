namespace Jarvis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAggregateForeignKeyToClient : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Clients", name: "Aggregate_ID", newName: "AggregateId");
            RenameIndex(table: "dbo.Clients", name: "IX_Aggregate_ID", newName: "IX_AggregateId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Clients", name: "IX_AggregateId", newName: "IX_Aggregate_ID");
            RenameColumn(table: "dbo.Clients", name: "AggregateId", newName: "Aggregate_ID");
        }
    }
}
