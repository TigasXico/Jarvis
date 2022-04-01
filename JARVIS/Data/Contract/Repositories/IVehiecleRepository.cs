
using Jarvis.Data.DataModels;

using System.Collections.Generic;
using System.Linq;

namespace Jarvis.DataAcess.Contract
{
    public interface IVehiecleRepository : IRepository<VehiecleDataModel>
    {
        //TODO Specify here vehiecle specific queries
        IEnumerable<IGrouping<FiscalEntityDataModel , VehiecleDataModel>> GetVehieclesWithPlateOnMonthForIUC( int targetMonth );
        
    }
}
