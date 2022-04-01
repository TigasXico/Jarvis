
using Jarvis.Data.Contract.Repositories;
using Jarvis.Data.DataAccess.Repositories;
using Jarvis.DataAccess.Database;
using Jarvis.DataAcess.Contract;

namespace Jarvis.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly JarvisContext context;

        public IClientRepository Clients
        {
            get;
            private set;
        }

        public ICompanyRepository Companies
        {
            get;
            private set;
        }

        public IContactsRepository Contacts
        {
            get;
            private set;
        }

        public IVehiecleRepository Vehiecles
        {
            get;
            private set;
        }

        public IRealEstateRepository RealEstates
        {
            get;
            private set;
        }

        public ICustomerGroupRepository CustomerGroups
        {
            get;
            private set;
        }

        public IAggregatesRepository Aggregates
        {
            get;
            private set;
        }

        public ITransactionRepository Transactions
        {
            get;
            private set;
        }

        public IImiChargeNotesRepository ImiChargeNotes
        {
            get;
            private set;
        }

        public UnitOfWork()
        {
            context = new JarvisContext();
            Clients = new ClientRepository( context );
            Companies = new CompanyRepository( context );
            Contacts = new ContactsRepository( context );
            Vehiecles = new VehiecleRepository( context );
            RealEstates = new RealEstateRepository( context );
            CustomerGroups = new CustomerGroupRepository( context );
            Aggregates = new AggregatesRepository( context );
            Transactions = new TransactionRepository( context );
            ImiChargeNotes = new ImiChargeNotesRepository( context );
        }

        public int Complete()
        {
            int updatedRecords = context.SaveChanges();

            return updatedRecords;
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
