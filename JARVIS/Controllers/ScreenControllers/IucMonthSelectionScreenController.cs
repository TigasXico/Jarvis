using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Input;

using GalaSoft.MvvmLight.CommandWpf;

using Jarvis.Services;

namespace Jarvis.Controllers.ScreenControllers
{
    public class IucMonthSelectionScreenController : PropertyRaiser
    {
        private IEnumerable<string> months;
        public IEnumerable<string> Months
        {
            get
            {
                if ( months == null )
                {
                    months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.Take( 12 ).ToList();
                }

                return months;
            }
        }

        private string selectedMonthName;
        public string SelectedMonthName
        {
            get => selectedMonthName;
            set => SetProperty( ref selectedMonthName , value );
        }

        private int selectedMonthNumber;
        public int SelectedMonthNumber
        {
            get => selectedMonthNumber;
            set => SetProperty( ref selectedMonthNumber , value + 1 );
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

        public IucMonthSelectionScreenController()
        {
            DisplayControlButtons = true;

            OkCommand = new RelayCommand( OkAction , CanClickOk );
            CancelCommand = new RelayCommand( CancelAction );
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
            return SelectedMonthNumber != -1;
        }

    }
}
