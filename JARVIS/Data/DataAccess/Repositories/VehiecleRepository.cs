using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using Jarvis.Data.DataModels;
using Jarvis.DataAccess.Database;
using Jarvis.DataAcess.Contract;

namespace Jarvis.DataAccess.Repositories
{
    internal class VehiecleRepository : Repository<VehiecleDataModel>, IVehiecleRepository
    {
        public JarvisContext CurrentContext => context as JarvisContext;

        public VehiecleRepository( JarvisContext context ) : base( context )
        {
        }

        public IEnumerable<IGrouping<FiscalEntityDataModel , VehiecleDataModel>> GetVehieclesWithPlateOnMonthForIUC( int targetMonth )
        {
            return dbSet
                .Where( v => v.DateOfLicensePlate.Value.Month == targetMonth )
                .Include( c => c.Owner )
                .Include( c => c.Owner.Contacts )
                .GroupBy( c => c.Owner )
                .OrderBy(c => c.Key.Name)
                .ToList();
        }
    }
}
