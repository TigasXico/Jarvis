
using Jarvis.Data.DataModels;

namespace Jarvis.DataAcess.Contract
{
    public interface IRealEstateRepository : IRepository<RealEstateDataModel>
    {
        //TODO Place here real estate related queries
        void RemoveRealEstatesOfClient( ClientDataModel clientToUpdate );
    }
}
