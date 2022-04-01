
using GalaSoft.MvvmLight.CommandWpf;

using Jarvis.Data.DataModels;
using Jarvis.Services;

namespace Jarvis.Controllers.ScreenControllers
{
    public class EditTransactionScreenController : BaseScreenController
    {
        private TransactionDataModel transaction;
        public TransactionDataModel Transaction
        {
            get => transaction;
            set => SetProperty(ref transaction , value);
        }

        public EditTransactionScreenController()
        {
            OkCommand = new RelayCommand( OkAction , CanClickOkCommand );
            CancelCommand = new RelayCommand( CancelAction );
        }

        private void OkAction()
        {
            WindowService.CloseWindowOfViewModel( this , true );
        }

        private bool CanClickOkCommand()
        {
            return Transaction != null 
                && !string.IsNullOrEmpty(Transaction?.TransactionName) 
                && Transaction?.Date != null
                && Transaction?.Amount != 0
                && Transaction?.TransactionType != TransactionType.Unknown;
        }

        private void CancelAction()
        {
            WindowService.CloseWindowOfViewModel( this , false );
        }
    }
}
