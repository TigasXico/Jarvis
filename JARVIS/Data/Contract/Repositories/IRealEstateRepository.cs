
using Jarvis.Data.DataModels;

namespace Jarvis.Data.Contract.Repositories
{
    public interface IRealEstateRepository : IRepository<RealEstateDataModel>
    {
        //TODO Place here real estate related queries
        void RemoveRealEstatesOfClient( ClientDataModel clientToUpdate );
    }
}
