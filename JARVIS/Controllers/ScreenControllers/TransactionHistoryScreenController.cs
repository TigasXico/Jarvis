using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

using GalaSoft.MvvmLight.CommandWpf;

using Jarvis.Controllers.ModelControllers;
using Jarvis.Data.DataModels;
using Jarvis.Services;

namespace Jarvis.Controllers.ScreenControllers
{
    public class TransactionHistoryScreenController : BaseScreenController
    {
        private readonly FiscalEntityDataModel fiscalEntity;

        private ObservableCollection<TransactionDataModel> transactions;
        public ObservableCollection<TransactionDataModel> Transactions
        {
            get => transactions;
            set => SetProperty( ref transactions , value );
        }

        public string CurrentBalance => fiscalEntity.CurrentBalance.ToString( "C2" );

        public ICommand AddTransactionCommand
        {
            get;
            private set;
        }

        public ICommand EditTransactionCommand
        {
            get;
            private set;
        }

        public ICommand RemoveTransactionCommand
        {
            get;
            private set;
        }

        private TransactionDataModel selectedTransaction;
        public TransactionDataModel SelectedTransaction
        {
            get => selectedTransaction;
            set => SetProperty( ref selectedTransaction , value );
        }

        public TransactionHistoryScreenController( FiscalEntityDataModel fiscalEntity )
        {
            this.fiscalEntity = fiscalEntity;

            Transactions = fiscalEntity.Transactions;

            OkCommand = new RelayCommand( OkAction );

            CancelCommand = new RelayCommand( CancelAction );

            AddTransactionCommand = new RelayCommand( AddTransactionAction );

            EditTransactionCommand = new RelayCommand( EditTransactionAction , CanEditTransaction );

            RemoveTransactionCommand = new RelayCommand( RemoveTransactionAction , CanRemoveTransaction );

            DisplayControlButtons = true;

            EnableControls = true;
        }

        private void AddTransactionAction()
        {
            EditTransactionScreenController newTransactionScreenController = new EditTransactionScreenController()
            {
                Transaction = new TransactionDataModel()
                {
                    Date = DateTime.Today
                } ,
                DisplayControlButtons = true
            };

            if ( WindowService.ShowWindowForController( newTransactionScreenController , "Adicionar nova transação" ) )
            {
                Transactions.Add( newTransactionScreenController.Transaction );
            }

            TransactionController.UpdateCurrentBalanceOfEntity( fiscalEntity );

            RaisePropertyChanged( nameof( CurrentBalance ) );
        }

        private void EditTransactionAction()
        {
            EditTransactionScreenController newTransactionScreenController = new EditTransactionScreenController()
            {
                Transaction = SelectedTransaction,
                DisplayControlButtons = true
            };

            WindowService.ShowWindowForController( newTransactionScreenController , "Adicionar nova transação" );

            TransactionController.UpdateCurrentBalanceOfEntity( fiscalEntity );

            RaisePropertyChanged( nameof( CurrentBalance ) );
        }

        private bool CanEditTransaction()
        {
            return SelectedTransaction != null;
        }

        private void RemoveTransactionAction()
        {
            Transactions.Remove( SelectedTransaction );

            TransactionController.UpdateCurrentBalanceOfEntity( fiscalEntity );

            RaisePropertyChanged( nameof( CurrentBalance ) );
        }

        private bool CanRemoveTransaction()
        {
            return SelectedTransaction != null;
        }

        private void OkAction()
        {
            WindowService.CloseWindowOfViewModel( this , true );
        }

        private void CancelAction()
        {
            WindowService.CloseWindowOfViewModel( this , false);
        }
    }
}
