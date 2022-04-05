using System.Windows.Input;

using GalaSoft.MvvmLight.CommandWpf;
using Jarvis.Controllers.Contract;
using Jarvis.Controllers.ModelControllers;
using Jarvis.Data.Contract;
using Jarvis.Data.DataModels;

namespace Jarvis.Controllers.ScreenControllers
{
    public class ClientScreenController : FiscalEntityScreenController, IDataModelScreenController<FiscalEntityDataModel>
    {
        public new IDataModel Entity => base.Entity;

        public ICommand UpdateAggregateCommand
        {
            get;
            private set;
        }

        public ClientScreenController( IUpdatableDataModelController<FiscalEntityDataModel> modelController ) : base( modelController )
        {
            UpdateAggregateCommand = new RelayCommand( UpdateAggregateAction );
        }

        private void UpdateAggregateAction()
        {
            if ( ClientController.GetAggregateSelection( DataModelController.UnitOfWork.Aggregates.GetAll() , out var selectedAggregateDataModel ) )
            {
                (Entity as ClientDataModel).Aggregate = selectedAggregateDataModel;
            }
        }
    }
}
