namespace Jarvis.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RemovedtagsFromRealEstatesAndVehiecles : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VehiecleTags", "VehiecleID", "dbo.Vehiecles");
            DropForeignKey("dbo.VehiecleTags", "TagID", "dbo.Tags");
            DropForeignKey("dbo.RealEstateTags", "RealEstateID", "dbo.RealEstates");
            DropForeignKey("dbo.RealEstateTags", "TagID", "dbo.Tags");
            DropIndex("dbo.VehiecleTags", new[] { "VehiecleID" });
            DropIndex("dbo.VehiecleTags", new[] { "TagID" });
            DropIndex("dbo.RealEstateTags", new[] { "RealEstateID" });
            DropIndex("dbo.RealEstateTags", new[] { "TagID" });
            DropTable("dbo.VehiecleTags");
            DropTable("dbo.RealEstateTags");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RealEstateTags",
                c => new
                    {
                        RealEstateID = c.Int(nullable: false),
                        TagID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RealEstateID, t.TagID });
            
            CreateTable(
                "dbo.VehiecleTags",
                c => new
                    {
                        VehiecleID = c.Int(nullable: false),
                        TagID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VehiecleID, t.TagID });
            
            CreateIndex("dbo.RealEstateTags", "TagID");
            CreateIndex("dbo.RealEstateTags", "RealEstateID");
            CreateIndex("dbo.VehiecleTags", "TagID");
            CreateIndex("dbo.VehiecleTags", "VehiecleID");
            AddForeignKey("dbo.RealEstateTags", "TagID", "dbo.Tags", "ID", cascadeDelete: true);
            AddForeignKey("dbo.RealEstateTags", "RealEstateID", "dbo.RealEstates", "ID", cascadeDelete: true);
            AddForeignKey("dbo.VehiecleTags", "TagID", "dbo.Tags", "ID", cascadeDelete: true);
            AddForeignKey("dbo.VehiecleTags", "VehiecleID", "dbo.Vehiecles", "ID", cascadeDelete: true);
        }
    }
}
