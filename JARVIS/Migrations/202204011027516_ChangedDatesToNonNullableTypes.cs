namespace Jarvis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedDatesToNonNullableTypes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ImiChargeNotes", "LimitDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Transactions", "Date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Vehiecles", "DateOfLicensePlate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vehiecles", "DateOfLicensePlate", c => c.DateTime());
            AlterColumn("dbo.Transactions", "Date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ImiChargeNotes", "LimitDate", c => c.DateTime(nullable: false));
        }
    }
}
