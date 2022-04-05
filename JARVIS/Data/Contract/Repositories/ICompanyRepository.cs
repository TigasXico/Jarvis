using System.Collections.Generic;
using Jarvis.Data.DataModels;

namespace Jarvis.Data.Contract.Repositories
{
    public interface ICompanyRepository : IRepository<CompanyDataModel>
    {
        //TODO Specify here company specific queries
        CompanyDataModel GetByFiscalNumber( string fiscalNumber );
        IEnumerable<FiscalEntityDataModel> GetCompaniesInInvalidState();
    }
}
