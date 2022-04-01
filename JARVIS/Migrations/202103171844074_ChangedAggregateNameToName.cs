namespace Jarvis.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ChangedAggregateNameToName : DbMigration
    {
        public override void Up()
        {
            RenameColumn( "dbo.Aggregates" , "AggregateName" , "Name");
        }
        
        public override void Down()
        {
            RenameColumn( "dbo.Aggregates" , "Name" , "AggregateName" );
        }
    }
}
