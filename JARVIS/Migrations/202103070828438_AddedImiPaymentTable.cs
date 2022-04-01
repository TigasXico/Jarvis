namespace Jarvis.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddedImiPaymentTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EntitiesImiPayments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PayingEntityId = c.Int(nullable: false),
                        PaymentPlan = c.Int(nullable: false),
                        YearOfPaymentPlan = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.FiscalEntities", t => t.PayingEntityId, cascadeDelete: true)
                .Index(t => t.PayingEntityId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EntitiesImiPayments", "PayingEntityId", "dbo.FiscalEntities");
            DropIndex("dbo.EntitiesImiPayments", new[] { "PayingEntityId" });
            DropTable("dbo.EntitiesImiPayments");
        }
    }
}
