using Jarvis.Controllers.ModelControllers;
using Jarvis.Data.DataModels;
using Jarvis.Interfaces;

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
