
using System;
using System.Collections.Generic;
using System.Linq;

using Jarvis.Data.DataModels;
using Jarvis.DataAccess.Database;
using Jarvis.DataAcess.Contract;

namespace Jarvis.DataAccess.Repositories
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
