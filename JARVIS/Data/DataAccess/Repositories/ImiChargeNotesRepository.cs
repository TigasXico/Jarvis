
using Jarvis.Data.Contract.Repositories;
using Jarvis.Data.DataAccess.Database;
using Jarvis.Data.DataModels;

namespace Jarvis.Data.DataAccess.Repositories
{
    internal class ImiChargeNotesRepository : Repository<ImiChargeNotesDataModel>, IImiChargeNotesRepository
    {
        public ImiChargeNotesRepository( JarvisContext context ) : base( context )
        {

        }
    }
}
