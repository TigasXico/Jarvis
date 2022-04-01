
using Jarvis.DataAccess.Database;
using Jarvis.DataAcess.Contract;
using Jarvis.DataModels;

namespace Jarvis.DataAccess.Repositories
{
    internal class RealEstateRepository : Repository<RealEstateDataModel>, IRealEstateRepository
    {
        public JarvisContext CurrentContext => Context as JarvisContext;

        public RealEstateRepository( JarvisContext context ) : base( context )
        {
        }

        public void RemoveRealEstatesOfClient( ClientDataModel clientToUpdate )
        {
            CurrentContext.RealEstates.RemoveRange( clientToUpdate.RealEstates );
        }
    }
}
