
using Jarvis.Data.DataModels;

namespace Jarvis.Data.Contract.Repositories
{
    public interface IAggregatesRepository : IRepository<AggregateDataModel>
    {
        AggregateDataModel GetByName( string aggregateName );
    }
}
