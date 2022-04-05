using System.ComponentModel;
using System.Windows.Input;

using GalaSoft.MvvmLight.Command;
using Jarvis.Controllers.Contract;
using Jarvis.Controllers.ModelControllers;
using Jarvis.Data.DataModels;
using Jarvis.Services;

namespace Jarvis.Controllers.ScreenControllers
{
    public class FiscalEntityScreenController : DataModelScreenController<FiscalEntityDataModel>
    {
        #region Properties

        private bool? validFiscalNumber;
        public bool? ValidFiscalNumber
        {
            get => validFiscalNumber;
            set => SetProperty( ref validFiscalNumber , value );
        }

        public string CurrentBalance => Entity.CurrentBalance.ToString( "C2" );

        private string selectedRelatedItem;
        public object SelectedRelatedItem
        {
            get => selectedRelatedItem;
            set => SetProperty( ref selectedRelatedItem , value?.ToString() );
        }

        public ICommand UpdateCustomerGroupCommand
        {
            get;
            private set;
        }

        public ICommand ShowTransactionsHistoryCommand
        {
            get;
            private set;
        }

        private CustomerGroupDataModel selectedCustomerGroup;
        public CustomerGroupDataModel SelectedCustomerGroup
        {
            get => selectedCustomerGroup;
            set => SetProperty( ref selectedCustomerGroup , value );
        }

        #endregion

        public FiscalEntityScreenController( IUpdatableDataModelController<FiscalEntityDataModel> controller ) : base(controller)
        {
            controller.Model.PropertyChanged += WrapperEntityPropertyChanged;

            EnableControls = true;

            ReadOnlyControlls = true;

            UpdateCustomerGroupCommand = new RelayCommand( UpdateCustomerGroupAction );

            ShowTransactionsHistoryCommand = new RelayCommand( ShowTransactionsHistoryAction );
        }

        private void UpdateCustomerGroupAction()
        {
            if ( FiscalEntityController.GetCustomerGroupSelection( DataModelController.UnitOfWork.CustomerGroups.GetAll() , out var selectedCustomerGroup ) )
            {
                Entity.CustomerGroup = selectedCustomerGroup;

                if ( selectedCustomerGroup != null )
                {
                    //TODO Change this to get max of available ID column of group
                    Entity.IdOnCustomerGroup = selectedCustomerGroup.CurrentIdCount++;
                }
                else
                {
                    Entity.IdOnCustomerGroup = null;
                }
            }
        }

        private void ShowTransactionsHistoryAction()
        {
            var screenController = new TransactionHistoryScreenController(Entity);

            WindowService.ShowWindowForController( screenController , "Histórico de pagamentos" );
        }

        private void WrapperEntityPropertyChanged( object sender , PropertyChangedEventArgs e )
        {
            if ( !WrappedObject.IsDirty )
            {
                WrappedObject.IsDirty = true;
            }

            if ( e.PropertyName == nameof( Entity.FiscalNumber ) )
            {
                ValidFiscalNumber = FiscalEntityController.IsFiscalNumberValid( Entity.FiscalNumber );
            }
        }
    }
}
