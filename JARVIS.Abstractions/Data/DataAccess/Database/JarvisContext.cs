
using System.Data.Entity;
using System.Data.Entity.SqlServer;

using Jarvis.DataModels;

namespace Jarvis.DataAccess.Database
{
    public class JarvisContext : DbContext
    {
        public JarvisContext() : base( "DefaultConnection" )
        {
            SqlProviderServices ensureDllIsCopied = SqlProviderServices.Instance;
        }

        public DbSet<ClientDataModel> Clients
        {
            get;
            set;
        }

        public DbSet<CompanyDataModel> Companies
        {
            get;
            set;
        }

        public DbSet<ContactDataModel> Contacts
        {
            get;
            set;
        }

        public DbSet<VehiecleDataModel> Vehiecles
        {
            get;
            set;
        }

        public DbSet<RealEstateDataModel> RealEstates
        {
            get;
            set;
        }

        public DbSet<TagDataModel> Tags
        {
            get;
            set;
        }

        protected override void OnModelCreating( DbModelBuilder modelBuilder )
        {
            #region Map models to tables

            modelBuilder.Entity<FiscalEntityDataModel>()
                .ToTable( "FiscalEntities" );

            modelBuilder.Entity<ClientDataModel>()
                .ToTable( "Clients" );

            modelBuilder.Entity<CompanyDataModel>()
                .ToTable( "Companies" );

            modelBuilder.Entity<ContactDataModel>()
                .ToTable( "Contacts" );

            modelBuilder.Entity<RealEstateDataModel>()
                .ToTable( "RealEstates" );

            modelBuilder.Entity<VehiecleDataModel>()
                .ToTable( "Vehiecles" );

            modelBuilder.Entity<TagDataModel>()
                .ToTable( "Tags" );

            #endregion

            #region FiscalEntity related setup

            #region Relations

            #region One-To-Many

            modelBuilder.Entity<FiscalEntityDataModel>()
                .HasMany( c => c.Vehiecles )
                .WithRequired( v => v.Owner )
                .HasForeignKey( v => v.OwnerId )
                .WillCascadeOnDelete( true );

            modelBuilder.Entity<FiscalEntityDataModel>()
                .HasMany( c => c.RealEstates )
                .WithRequired( re => re.Owner )
                .HasForeignKey( re => re.OwnerId )
                .WillCascadeOnDelete( true );

            modelBuilder.Entity<FiscalEntityDataModel>()
                .HasMany( c => c.Contacts )
                .WithRequired( c => c.ContactHolder )
                .HasForeignKey( c => c.ContactHolderId )
                .WillCascadeOnDelete( true );

            #endregion

            #region Many-To-Many

            modelBuilder.Entity<FiscalEntityDataModel>()
                    .HasMany( e => e.Tags )
                    .WithMany( t => t.FiscalEntities )
                    .Map( entityTags =>
                    {
                        entityTags.ToTable( "FiscalEntityTags" );
                        entityTags.MapLeftKey( "FiscalEntityID" );
                        entityTags.MapRightKey( "TagID" );
                    } );

            #endregion

            #endregion

            #endregion

            #region Client related setup

            modelBuilder.Entity<ClientDataModel>()
                .Property( c => c.FiscalNumber )
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength( 9 );

            modelBuilder.Entity<ClientDataModel>()
                .Property( c => c.FiscalPassword )
                .IsRequired()
                .HasMaxLength( 255 );

            modelBuilder.Entity<ClientDataModel>()
                .Property( c => c.Notes )
                .HasMaxLength( 2000 );

            #endregion

            #region Vehiecle related setup

            #region Relations

            #region Many-To-Many

            modelBuilder.Entity<VehiecleDataModel>()
                    .HasMany( v => v.Tags )
                    .WithMany( t => t.Vehiecles )
                    .Map( vehiecleTags =>
                    {
                        vehiecleTags.ToTable( "VehiecleTags" );
                        vehiecleTags.MapLeftKey( "VehiecleID" );
                        vehiecleTags.MapRightKey( "TagID" );
                    } );

            #endregion

            #endregion

            #endregion

            #region Real Estate related setup

            #region Relations

            #region Many-To-Many

            modelBuilder.Entity<RealEstateDataModel>()
                    .HasMany( c => c.Tags )
                    .WithMany( t => t.RealEstates )
                    .Map( realEstateTags =>
                    {
                        realEstateTags.ToTable( "RealEstateTags" );
                        realEstateTags.MapLeftKey( "RealEstateID" );
                        realEstateTags.MapRightKey( "TagID" );
                    } );

            #endregion

            #endregion

            #endregion

            base.OnModelCreating( modelBuilder );

        }

    }
}
