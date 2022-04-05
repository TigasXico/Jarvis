using System;
using System.Collections.Generic;
using System.Linq;
using Jarvis.Data.Contract.Repositories;
using Jarvis.Data.DataAccess.Database;
using Jarvis.Data.DataModels;

namespace Jarvis.Data.DataAccess.Repositories
{
    internal class ClientRepository : Repository<ClientDataModel>, IClientRepository
    {
        public ClientRepository( JarvisContext context ) : base( context )
        {
        }

        public ClientDataModel GetByFiscalNumber( string fiscalNumber )
        {
            return Get( c => c.FiscalNumber.Equals( fiscalNumber , StringComparison.InvariantCultureIgnoreCase ) ).FirstOrDefault();
        }

        public IEnumerable<FiscalEntityDataModel> GetClientsInInvalidState()
        {
            return Get( c => (c.CurrentStatus != FiscalEntityStatus.Updated && c.CurrentStatus != FiscalEntityStatus.NotUpdated) );
        }
    }
}
