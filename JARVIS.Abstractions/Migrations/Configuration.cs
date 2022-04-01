namespace Jarvis.Migrations
{
    using System.Data.Entity.Migrations;

    using Jarvis.DataAccess.Database;

    internal sealed class Configuration : DbMigrationsConfiguration<JarvisContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;

        }

        protected override void Seed( JarvisContext context )
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
