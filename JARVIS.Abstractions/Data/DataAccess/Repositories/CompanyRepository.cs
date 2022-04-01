
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;

using Jarvis.DataAccess.Database;
using Jarvis.DataAcess.Contract;
using Jarvis.DataModels;

namespace Jarvis.DataAccess.Repositories
{
    internal class CompanyRepository : Repository<CompanyDataModel>, ICompanyRepository
    {
        public JarvisContext CurrentContext => Context as JarvisContext;

        public CompanyRepository( JarvisContext context ) : base( context )
        {
        }

        public bool GetByFiscalNumber( string fiscalNumber , out CompanyDataModel foundCompany )
        {
            foundCompany = CurrentContext.Companies.SingleOrDefault( c => c.FiscalNumber.Equals( fiscalNumber , StringComparison.InvariantCultureIgnoreCase ) );
            return foundCompany != default( CompanyDataModel );
        }

        public void GetCompanyRelatedItems( CompanyDataModel company )
        {
            //CurrentContext.Companies.Attach( company );

            DbEntityEntry<CompanyDataModel> entry = CurrentContext.Entry( company );

            entry.Collection( c => c.Contacts ).Load();
            entry.Collection( c => c.Vehiecles ).Load();
            entry.Collection( c => c.RealEstates ).Load();
            entry.Collection( c => c.Tags ).Load();
        }
    }
}
