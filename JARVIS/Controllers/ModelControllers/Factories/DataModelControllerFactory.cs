using System.ComponentModel;

using Jarvis.Data.DataModels;
using Jarvis.DataAcess.Contract;
using Jarvis.Interfaces;

namespace Jarvis.Controllers.ModelControllers.Factories
{
    public static class DataModelControllerFactory
    {
        internal static IUpdatableDataModelController<FiscalEntityDataModel> GetControllerForEntity( IFiscalEntity entity , BackgroundWorker worker = null , IUnitOfWork unitOfWork = null )
        {
            IUpdatableDataModelController<FiscalEntityDataModel> controller = null;

            switch ( entity )
            {
                case ClientDataModel client:
                    controller = new ClientController( client , worker , unitOfWork );
                    break;
                case CompanyDataModel company:
                    controller = new CompanyController( company , worker , unitOfWork );
                    break;
            }

            return controller;
        }
    }
}
