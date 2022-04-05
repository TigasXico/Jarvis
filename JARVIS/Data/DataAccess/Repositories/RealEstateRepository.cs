using Jarvis.Data.Contract.Repositories;
using Jarvis.Data.DataAccess.Database;
using Jarvis.Data.DataModels;

namespace Jarvis.Data.DataAccess.Repositories
{
    internal class RealEstateRepository : Repository<RealEstateDataModel>, IRealEstateRepository
    {
        public RealEstateRepository( JarvisContext context ) : base( context )
        {
        }

        public void RemoveRealEstatesOfClient( ClientDataModel clientToUpdate )
        {
            dbSet.RemoveRange( clientToUpdate.RealEstates );
        }
    }
}
