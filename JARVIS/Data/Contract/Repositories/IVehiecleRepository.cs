using System.Collections.Generic;
using System.Linq;
using Jarvis.Data.DataModels;

namespace Jarvis.Data.Contract.Repositories
{
    public interface IVehiecleRepository : IRepository<VehiecleDataModel>
    {
        //TODO Specify here vehiecle specific queries
        IEnumerable<IGrouping<FiscalEntityDataModel , VehiecleDataModel>> GetVehieclesWithPlateOnMonthForIUC( int targetMonth );
        
    }
}
