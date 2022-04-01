
using System.Collections.Generic;

using Jarvis.Data.DataModels;

namespace Jarvis.DataAcess.Contract
{
    public interface IClientRepository : IRepository<ClientDataModel>
    {
        //TODO Specify here client specific queries
        ClientDataModel GetByFiscalNumber( string fiscalNumber );
        IEnumerable<FiscalEntityDataModel> GetClientsInInvalidState();
    }
}
