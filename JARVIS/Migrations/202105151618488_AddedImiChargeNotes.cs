namespace Jarvis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedImiChargeNotes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ImiPayments", "PayingEntityId", "dbo.FiscalEntities");
            DropIndex("dbo.ImiPayments", new[] { "PayingEntityId" });
            CreateTable(
                "dbo.ImiChargeNotes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PayingEntityId = c.Int(nullable: false),
                        Year = c.String(),
                        ChargeNoteNumber = c.String(),
                        PaymentValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LimitDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        PaymentReference = c.String(),
                        NumberOfBuildings = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.FiscalEntities", t => t.PayingEntityId, cascadeDelete: true)
                .Index(t => t.PayingEntityId);
            
            DropTable("dbo.ImiPayments");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ImiPayments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PayingEntityId = c.Int(nullable: false),
                        PaymentPlan = c.Int(nullable: false),
                        YearOfPaymentPlan = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.ImiChargeNotes", "PayingEntityId", "dbo.FiscalEntities");
            DropIndex("dbo.ImiChargeNotes", new[] { "PayingEntityId" });
            DropTable("dbo.ImiChargeNotes");
            CreateIndex("dbo.ImiPayments", "PayingEntityId");
            AddForeignKey("dbo.ImiPayments", "PayingEntityId", "dbo.FiscalEntities", "ID", cascadeDelete: true);
        }
    }
}
