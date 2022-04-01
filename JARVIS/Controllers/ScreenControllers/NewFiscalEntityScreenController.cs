using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

using GalaSoft.MvvmLight.CommandWpf;

using Jarvis.Controllers.ModelControllers;
using Jarvis.Data.DataModels;
using Jarvis.DataAcess.Contract;
using Jarvis.Interfaces;
using Jarvis.Services;

namespace Jarvis.Controllers.ScreenControllers
{
    public class NewFiscalEntityScreenController : PropertyRaiser, IDismissable
    {
        private ObservableCollection<FiscalEntityCredentials> credentialsBeingEdited;
        private readonly IUnitOfWork unitOfWork;

        public ObservableCollection<FiscalEntityCredentials> CredentialsBeingEdited
        {
            get => credentialsBeingEdited;
            set => SetProperty( ref credentialsBeingEdited , value );
        }

        private FiscalEntityCredentials selectedCredentials;
        public FiscalEntityCredentials SelectedCredentials
        {
            get => selectedCredentials;
            set => SetProperty( ref selectedCredentials , value );
        }

        private AggregateDataModel selectedAggregate;
        public AggregateDataModel SelectedAggregate
        {
            get => selectedAggregate;
            set => SetProperty( ref selectedAggregate , value );
        }

        private CustomerGroupDataModel selectedCustomerGroup;
        public CustomerGroupDataModel SelectedCustomerGroup
        {
            get => selectedCustomerGroup;
            set => SetProperty(ref selectedCustomerGroup , value );
        }


        public ICommand UpdateAggregateCommand
        {
            get;
            private set;
        }

        public ICommand UpdateCustomerGroupCommand
        {
            get;
            private set;
        }

        public ICommand AddEntityCommand
        {
            get;
            private set;
        }

        public ICommand RemoveEntityCommand
        {
            get;
            private set;
        }

        public ICommand OkCommand
        {
            get;
            private set;
        }

        public ICommand CancelCommand
        {
            get;
            private set;
        }

        private bool displayControlButtons;
        public bool DisplayControlButtons
        {
            get => displayControlButtons;
            set => SetProperty( ref displayControlButtons , value );
        }

        public bool AllCredentialsValid => CredentialsBeingEdited.All( fe => FiscalEntityController.IsFiscalNumberValid( fe.FiscalNumber ) && !string.IsNullOrWhiteSpace( fe.FiscalPassword ) );

        public NewFiscalEntityScreenController( IUnitOfWork unitOfWork )
        {
            DisplayControlButtons = true;

            AddEntityCommand = new RelayCommand( AddEntityAction );
            RemoveEntityCommand = new RelayCommand( RemoveEntityAction , CanRemoveEntity );

            UpdateAggregateCommand = new RelayCommand( UpdateAggregateAction );
            UpdateCustomerGroupCommand = new RelayCommand( UpdateCustomerGroupAction );

            OkCommand = new RelayCommand( OkAction , CanClickOk );
            CancelCommand = new RelayCommand( CancelAction );

            this.unitOfWork = unitOfWork;

            CredentialsBeingEdited = new ObservableCollection<FiscalEntityCredentials>();

            CredentialsBeingEdited.CollectionChanged += ( sender , ev ) =>
            {
                if ( ev.NewItems != null )
                {
                    IList newItems = ev.NewItems;

                    foreach ( object newItem in newItems )
                    {
                        if ( newItem is INotifyPropertyChanged raiserAdd )
                        {
                            raiserAdd.PropertyChanged += CredentialsUpdateEventHandler;
                        }
                    }
                }

                if ( ev.OldItems != null )
                {
                    IList oldItems = ev.OldItems;

                    foreach ( object oldItem in oldItems )
                    {
                        if ( oldItem is INotifyPropertyChanged raiserRemove )
                        {
                            raiserRemove.PropertyChanged -= CredentialsUpdateEventHandler;
                        }
                    }
                }
            };
            CredentialsBeingEdited.Add( new FiscalEntityCredentials() );
        }

        private void UpdateAggregateAction()
        {
            if ( ClientController.GetAggregateSelection( unitOfWork.Aggregates.GetAll() , out AggregateDataModel selectedAggregateDataModel ) )
            {
                SelectedAggregate = selectedAggregateDataModel;
            }
        }

        private void UpdateCustomerGroupAction()
        {
            if(FiscalEntityController.GetCustomerGroupSelection( unitOfWork.CustomerGroups.GetAll() , out CustomerGroupDataModel selectedCustomerGroup ) )
            {
                SelectedCustomerGroup = selectedCustomerGroup;
            }
        }

        private void AddEntityAction()
        {
            CredentialsBeingEdited.Add( new FiscalEntityCredentials() );
        }

        private void RemoveEntityAction()
        {
            CredentialsBeingEdited.Remove( SelectedCredentials );
        }

        private bool CanRemoveEntity()
        {
            return SelectedCredentials != null;
        }

        private void OkAction()
        {
            WindowService.CloseWindowOfViewModel( this , true );
        }

        private void CancelAction()
        {
            WindowService.CloseWindowOfViewModel( this , false );
        }

        private bool CanClickOk()
        {
            return CredentialsBeingEdited?.Count > 0 && AllCredentialsValid;
        }


        private void CredentialsUpdateEventHandler( object sender , PropertyChangedEventArgs e )
        {
            (OkCommand as RelayCommand).RaiseCanExecuteChanged();
        }
    }

    public class FiscalEntityCredentials : PropertyRaiser
    {
        private string fiscalNumber;

        public string FiscalNumber
        {
            get => fiscalNumber;
            set => SetProperty( ref fiscalNumber , value );
        }

        private string fiscalPassword;

        public string FiscalPassword
        {
            get => fiscalPassword;
            set => SetProperty( ref fiscalPassword , value );
        }
    }
}

