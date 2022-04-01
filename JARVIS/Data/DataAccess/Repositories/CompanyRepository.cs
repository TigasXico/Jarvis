
using System;
using System.Collections.Generic;
using System.Linq;

using Jarvis.Data.DataModels;
using Jarvis.DataAccess.Database;
using Jarvis.DataAcess.Contract;

namespace Jarvis.DataAccess.Repositories
{
    internal class CompanyRepository : Repository<CompanyDataModel>, ICompanyRepository
    {
        public CompanyRepository( JarvisContext context ) : base( context )
        {
        }

        public CompanyDataModel GetByFiscalNumber( string fiscalNumber )
        {
            return Get( c => c.FiscalNumber.Equals(fiscalNumber , StringComparison.InvariantCultureIgnoreCase ) ).FirstOrDefault();
        }

        public IEnumerable<FiscalEntityDataModel> GetCompaniesInInvalidState()
        {
            return Get( c => (c.CurrentStatus != FiscalEntityStatus.Updated && c.CurrentStatus != FiscalEntityStatus.NotUpdated) );
        }
    }
}
