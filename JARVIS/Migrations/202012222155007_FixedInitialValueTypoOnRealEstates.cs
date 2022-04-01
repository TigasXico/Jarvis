namespace Jarvis.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class FixedInitialValueTypoOnRealEstates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RealEstates", "InitialValue", c => c.String());
            DropColumn("dbo.RealEstates", "IntialValue");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RealEstates", "IntialValue", c => c.String());
            DropColumn("dbo.RealEstates", "InitialValue");
        }
    }
}
