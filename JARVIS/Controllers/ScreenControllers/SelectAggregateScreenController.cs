
using System.Collections.Generic;

using Jarvis.Data.DataModels;

namespace Jarvis.Controllers.ScreenControllers
{
    public class SelectAggregateScreenController : SelectFromMultipleItemsScreenController<AggregateDataModel>
    {
        public SelectAggregateScreenController( IEnumerable<AggregateDataModel> existingItems ) : base(existingItems)
        {

        }
    }
}
