namespace Jarvis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FiscalEntities",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FiscalNumber = c.String(nullable: false, maxLength: 9, fixedLength: true),
                        FiscalPassword = c.String(nullable: false, maxLength: 255),
                        FiscalAddress = c.String(),
                        FiscalAddressAdditionalInfo = c.String(),
                        FiscalAddressZipCode = c.String(),
                        FinancialServicesRepartition = c.String(),
                        Notes = c.String(maxLength: 2000),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ContactValue = c.String(),
                        ContactType = c.Int(nullable: false),
                        ContactHolderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.FiscalEntities", t => t.ContactHolderId, cascadeDelete: true)
                .Index(t => t.ContactHolderId);
            
            CreateTable(
                "dbo.RealEstates",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OwnerId = c.Int(nullable: false),
                        Location = c.String(),
                        FullArticle = c.String(),
                        Type = c.String(),
                        Article = c.String(),
                        Fraction = c.String(),
                        Part = c.String(),
                        MatrixYear = c.Int(),
                        IntialValue = c.String(),
                        CurrentValue = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.FiscalEntities", t => t.OwnerId)
                .Index(t => t.OwnerId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Vehiecles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OwnerId = c.Int(nullable: false),
                        RoleOfClient = c.String(),
                        LicensePlate = c.String(),
                        DateOfLicensePlate = c.DateTime(),
                        Brand = c.String(),
                        Model = c.String(),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.FiscalEntities", t => t.OwnerId)
                .Index(t => t.OwnerId);
            
            CreateTable(
                "dbo.VehiecleTags",
                c => new
                    {
                        VehiecleID = c.Int(nullable: false),
                        TagID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VehiecleID, t.TagID })
                .ForeignKey("dbo.Vehiecles", t => t.VehiecleID, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagID, cascadeDelete: true)
                .Index(t => t.VehiecleID)
                .Index(t => t.TagID);
            
            CreateTable(
                "dbo.RealEstateTags",
                c => new
                    {
                        RealEstateID = c.Int(nullable: false),
                        TagID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RealEstateID, t.TagID })
                .ForeignKey("dbo.RealEstates", t => t.RealEstateID, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagID, cascadeDelete: true)
                .Index(t => t.RealEstateID)
                .Index(t => t.TagID);
            
            CreateTable(
                "dbo.FiscalEntityTags",
                c => new
                    {
                        FiscalEntityID = c.Int(nullable: false),
                        TagID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.FiscalEntityID, t.TagID })
                .ForeignKey("dbo.FiscalEntities", t => t.FiscalEntityID, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagID, cascadeDelete: true)
                .Index(t => t.FiscalEntityID)
                .Index(t => t.TagID);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        BirthDate = c.DateTime(),
                        Gender = c.String(),
                        Nationality = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.FiscalEntities", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.FiscalEntities", t => t.ID)
                .Index(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Companies", "ID", "dbo.FiscalEntities");
            DropForeignKey("dbo.Clients", "ID", "dbo.FiscalEntities");
            DropForeignKey("dbo.Vehiecles", "OwnerId", "dbo.FiscalEntities");
            DropForeignKey("dbo.FiscalEntityTags", "TagID", "dbo.Tags");
            DropForeignKey("dbo.FiscalEntityTags", "FiscalEntityID", "dbo.FiscalEntities");
            DropForeignKey("dbo.RealEstates", "OwnerId", "dbo.FiscalEntities");
            DropForeignKey("dbo.RealEstateTags", "TagID", "dbo.Tags");
            DropForeignKey("dbo.RealEstateTags", "RealEstateID", "dbo.RealEstates");
            DropForeignKey("dbo.VehiecleTags", "TagID", "dbo.Tags");
            DropForeignKey("dbo.VehiecleTags", "VehiecleID", "dbo.Vehiecles");
            DropForeignKey("dbo.Contacts", "ContactHolderId", "dbo.FiscalEntities");
            DropIndex("dbo.Companies", new[] { "ID" });
            DropIndex("dbo.Clients", new[] { "ID" });
            DropIndex("dbo.FiscalEntityTags", new[] { "TagID" });
            DropIndex("dbo.FiscalEntityTags", new[] { "FiscalEntityID" });
            DropIndex("dbo.RealEstateTags", new[] { "TagID" });
            DropIndex("dbo.RealEstateTags", new[] { "RealEstateID" });
            DropIndex("dbo.VehiecleTags", new[] { "TagID" });
            DropIndex("dbo.VehiecleTags", new[] { "VehiecleID" });
            DropIndex("dbo.Vehiecles", new[] { "OwnerId" });
            DropIndex("dbo.RealEstates", new[] { "OwnerId" });
            DropIndex("dbo.Contacts", new[] { "ContactHolderId" });
            DropTable("dbo.Companies");
            DropTable("dbo.Clients");
            DropTable("dbo.FiscalEntityTags");
            DropTable("dbo.RealEstateTags");
            DropTable("dbo.VehiecleTags");
            DropTable("dbo.Vehiecles");
            DropTable("dbo.Tags");
            DropTable("dbo.RealEstates");
            DropTable("dbo.Contacts");
            DropTable("dbo.FiscalEntities");
        }
    }
}
