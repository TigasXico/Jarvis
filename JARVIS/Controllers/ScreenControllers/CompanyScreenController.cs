using Jarvis.Controllers.Contract;
using Jarvis.Controllers.ModelControllers;
using Jarvis.Data.Contract;
using Jarvis.Data.DataModels;

namespace Jarvis.Controllers.ScreenControllers
{
    public class CompanyScreenController : FiscalEntityScreenController, IDataModelScreenController<FiscalEntityDataModel>
    {
        public new IDataModel Entity => base.Entity;

        public CompanyScreenController( IUpdatableDataModelController<FiscalEntityDataModel> controller ) : base( controller )
        {

        }
    }
}
