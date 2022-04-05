using System.Data.Entity;
using Jarvis.Data.DataModels;

namespace Jarvis.Data.DataAccess.Database
{
    public class JarvisContext : DbContext
    {
        public JarvisContext() : base( "DefaultConnection" )
        {
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

        public DbSet<CustomerGroupDataModel> CustomerGroups
        {
            get;
            set;
        }

        public DbSet<ImiChargeNotesDataModel> ImiChargeNotes
        {
            get;
            set;
        }

        public DbSet<AggregateDataModel> Aggregates
        {
            get;
            set;
        }

        public DbSet<TransactionDataModel> Transactions
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

            modelBuilder.Entity<CustomerGroupDataModel>()
                .ToTable( "CustomerGroups" );

            modelBuilder.Entity<ImiChargeNotesDataModel>()
                .ToTable( "EntitiesImiPayments" );

            modelBuilder.Entity<AggregateDataModel>()
                .ToTable( "Aggregates" );

            modelBuilder.Entity<ImiChargeNotesDataModel>()
                .ToTable( "ImiChargeNotes" );

            modelBuilder.Entity<TransactionDataModel>()
                .ToTable( "Transactions" );

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

            modelBuilder.Entity<FiscalEntityDataModel>()
                .HasMany( c => c.ImiChargeNotes )
                .WithRequired( cn => cn.PayingEntity )
                .HasForeignKey( cn => cn.PayingEntityId )
                .WillCascadeOnDelete( true );

            modelBuilder.Entity<FiscalEntityDataModel>()
                .HasMany( c => c.ImiChargeNotes )
                .WithRequired( pp => pp.PayingEntity )
                .HasForeignKey( c => c.PayingEntityId )
                .WillCascadeOnDelete( true );

            #endregion

            #region Many-To-Many

            modelBuilder.Entity<FiscalEntityDataModel>()
                    .HasOptional( e => e.CustomerGroup )
                    .WithMany( t => t.FiscalEntities )
                    .HasForeignKey( e => e.CustomerGroupId )
                    .WillCascadeOnDelete(false);

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

            modelBuilder.Entity<ClientDataModel>()
                .HasOptional(c => c.Aggregate)
                .WithMany(a => a.Members)
                .HasForeignKey(c => c.AggregateId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Vehiecle related setup

            #region Relations

            #region Many-To-Many

            #endregion

            #endregion

            #endregion

            #region Real Estate related setup

            #region Relations

            #region Many-To-Many

            #endregion

            #endregion

            #endregion

            base.OnModelCreating( modelBuilder );

        }

    }
}
