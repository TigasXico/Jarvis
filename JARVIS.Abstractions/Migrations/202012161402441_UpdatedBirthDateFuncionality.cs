namespace Jarvis.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UpdatedBirthDateFuncionality : DbMigration
    {
        public override void Up()
        {
            AlterColumn( "dbo.Clients" , "BirthDate" , c => c.String() );
        }

        public override void Down()
        {
            AlterColumn( "dbo.Clients" , "BirthDate" , c => c.DateTime() );
        }
    }
}
