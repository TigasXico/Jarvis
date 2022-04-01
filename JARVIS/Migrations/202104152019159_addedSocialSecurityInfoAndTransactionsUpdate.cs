namespace Jarvis.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addedSocialSecurityInfoAndTransactionsUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RequestorId = c.Int(nullable: false),
                        TransactionName = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransactionType = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.FiscalEntities", t => t.RequestorId, cascadeDelete: false)
                .Index(t => t.RequestorId);
            
            AddColumn("dbo.Clients", "IsDependent", c => c.Boolean());
            AddColumn("dbo.FiscalEntities", "SocialSecurityNumber", c => c.String());
            AddColumn("dbo.FiscalEntities", "SocialSecurityPassword", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "RequestorId", "dbo.FiscalEntities");
            DropIndex("dbo.Transactions", new[] { "RequestorId" });
            DropColumn("dbo.FiscalEntities", "SocialSecurityPassword");
            DropColumn("dbo.FiscalEntities", "SocialSecurityNumber");
            DropColumn("dbo.Clients", "IsDependent");
            DropTable("dbo.Transactions");
        }
    }
}
