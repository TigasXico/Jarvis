
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;

using Jarvis.DataAccess.Database;
using Jarvis.DataAcess.Contract;
using Jarvis.DataModels;
using Jarvis.Services;

namespace Jarvis.DataAccess.Repositories
{
    internal class ClientRepository : Repository<ClientDataModel>, IClientRepository
    {
        public JarvisContext CurrentContext => Context as JarvisContext;

        public ClientRepository( JarvisContext context ) : base( context )
        {
        }

        public bool TryGetByFiscalNumber( string fiscalNumber , out ClientDataModel foundClient )
        {
            foundClient = CurrentContext.Clients.SingleOrDefault( c => c.FiscalNumber.Equals( fiscalNumber , StringComparison.InvariantCultureIgnoreCase ) );

            return foundClient != default( ClientDataModel );
        }

        public void GetClientRelatedItems( ClientDataModel client )
        {
            //CurrentContext.Clients.Attach( client );
            try
            {
                DbEntityEntry<ClientDataModel> entry = CurrentContext.Entry( client );

                entry.Collection( c => c.Tags ).Load();
                entry.Collection( c => c.Contacts ).Load();
                entry.Collection( c => c.Vehiecles ).Load();
                entry.Collection( c => c.RealEstates ).Load();
            }
            catch ( Exception ex )
            {
                WindowService.DisplayMessage( MessageType.Error , ex.ToString() );
                throw;
            }
        }
    }
}
