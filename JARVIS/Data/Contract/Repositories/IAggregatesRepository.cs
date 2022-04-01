
using Jarvis.Data.DataModels;
using Jarvis.DataAcess.Contract;

namespace Jarvis.Data.Contract.Repositories
{
    public interface IAggregatesRepository : IRepository<AggregateDataModel>
    {
        AggregateDataModel GetByName( string aggregateName );
    }
}
