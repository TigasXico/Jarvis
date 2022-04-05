
using Jarvis.Data.DataModels;

namespace Jarvis.Data.Contract.Repositories
{
    public interface ICustomerGroupRepository : IRepository<CustomerGroupDataModel>
    {
        //TODO Specify here customer group specific queries
        CustomerGroupDataModel GetByName( string customerGroupName );
    }
}
