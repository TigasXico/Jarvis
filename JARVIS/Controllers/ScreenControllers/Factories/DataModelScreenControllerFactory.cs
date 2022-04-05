using Jarvis.Controllers.Contract;
using Jarvis.Controllers.ModelControllers;
using Jarvis.Data.DataModels;

namespace Jarvis.Controllers.ScreenControllers.Factories
{
    internal class DataModelScreenControllerFactory
    {
        internal static IDataModelScreenController<FiscalEntityDataModel> GetScreenControllerForEntity( IDataModelController<FiscalEntityDataModel> controller )
        {
            IDataModelScreenController<FiscalEntityDataModel> screenController = null;

            switch ( controller )
            {
                case ClientController clientController:
                    screenController = new ClientScreenController( clientController );
                    break;
                case CompanyController companyController:
                    screenController = new CompanyScreenController( companyController );
                    break;
            }

            return screenController;
        }
    }
}