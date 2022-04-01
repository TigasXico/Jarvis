
using Jarvis.Data.DataModels;
using Jarvis.DataAccess.Database;
using Jarvis.DataAcess.Contract;

namespace Jarvis.DataAccess.Repositories
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
