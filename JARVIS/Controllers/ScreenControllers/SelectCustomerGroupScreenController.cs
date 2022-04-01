using System.Collections.Generic;

using Jarvis.Data.DataModels;

namespace Jarvis.Controllers.ScreenControllers
{
    internal class SelectCustomerGroupScreenController : SelectFromMultipleItemsScreenController<CustomerGroupDataModel>
    {
        public SelectCustomerGroupScreenController( IEnumerable<CustomerGroupDataModel> existingItems ) : base( existingItems )
        {

        }
    }
}
