using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using Jarvis.DataAccess.Database;
using Jarvis.DataAcess.Contract;
using Jarvis.DataModels;

namespace Jarvis.DataAccess.Repositories
{
    internal class VehiecleRepository : Repository<VehiecleDataModel>, IVehiecleRepository
    {
        public JarvisContext CurrentContext => Context as JarvisContext;

        public VehiecleRepository( JarvisContext context ) : base( context )
        {
        }

        public IEnumerable<VehiecleDataModel> GetVehieclesForIucOnMonthWithOwnerAndContacts( int targetMonth )
        {
            return CurrentContext.Vehiecles.Where( v => v.DateOfLicensePlate.HasValue && v.DateOfLicensePlate.Value.Month == targetMonth )
                   .Include( v => v.Owner )
                   .Include( v => v.Owner.Contacts )
                   .OrderBy( v => v.Owner.Name )
                   .ToList();
        }
    }
}
