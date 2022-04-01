namespace Jarvis.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CascadeDeleteOfFiscalEntity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RealEstates", "OwnerId", "dbo.FiscalEntities");
            DropForeignKey("dbo.Vehiecles", "OwnerId", "dbo.FiscalEntities");
            AddForeignKey("dbo.RealEstates", "OwnerId", "dbo.FiscalEntities", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Vehiecles", "OwnerId", "dbo.FiscalEntities", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vehiecles", "OwnerId", "dbo.FiscalEntities");
            DropForeignKey("dbo.RealEstates", "OwnerId", "dbo.FiscalEntities");
            AddForeignKey("dbo.Vehiecles", "OwnerId", "dbo.FiscalEntities", "ID");
            AddForeignKey("dbo.RealEstates", "OwnerId", "dbo.FiscalEntities", "ID");
        }
    }
}
