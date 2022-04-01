using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using GalaSoft.MvvmLight.Command;

using Jarvis.Data.DataModels;
using Jarvis.DataAccess.Repositories;
using Jarvis.DataAcess.Contract;
using Jarvis.Interfaces;
using Jarvis.Services;
using Jarvis.Utils.HelperClasses;

namespace Jarvis.Controllers.ScreenControllers
{
    public class SearchScreenController : ObjectWrapper<SelectionGridScreenController>, IDismissable
    {
        private SearchMode searchMode;
        public SearchMode SearchMode
        {
            get => searchMode;
            set
            {
                if ( SetProperty( ref searchMode , value ) )
                {
                    SearchModeChanged();
                }
            }
        }

        private bool displayControlButtons;
        public bool DisplayControlButtons
        {
            get => displayControlButtons;
            private set => SetProperty( ref displayControlButtons , value );

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

        public List<IDataModel> SelectedEntries
        {
            get;
            set;
        }

        public SearchScreenController() : base( new SelectionGridScreenController() )
        {
            DisplayControlButtons = true;

            WrappedObject.AllowMultipleSelection = true;

            WrappedObject.Filter = ShouldShowItem;

            OkCommand = new RelayCommand( OkAction );

            CancelCommand = new RelayCommand( CancelAction );
        }

        private void OkAction()
        {
            SelectedEntries = WrappedObject.GetSelectedItems();

            WindowService.CloseWindowOfViewModel( this , true );

        }

        private void CancelAction()
        {
            SelectedEntries = null;

            WindowService.CloseWindowOfViewModel( this , false );
        }

        private void SearchModeChanged()
        {
            IEnumerable<IDataModel> currentWorkSet = null;

            //TODO: Check if the unit of work can be disposed here
            //after loading the entities
            IUnitOfWork unitOfWork = new UnitOfWork();

            switch ( SearchMode )
            {
                case SearchMode.SearchFiscalEntity:
                    currentWorkSet = unitOfWork.Clients.GetAll();
                    currentWorkSet = currentWorkSet.Union( unitOfWork.Companies.GetAll() );
                    break;
                case SearchMode.SearchClient:
                    currentWorkSet = unitOfWork.Clients.GetAll();
                    break;
                case SearchMode.SearchCompany:
                    currentWorkSet = unitOfWork.Companies.GetAll();
                    break;
                case SearchMode.SearchVehiecle:
                    currentWorkSet = unitOfWork.Vehiecles.GetAll();
                    break;
                case SearchMode.SearchRealEstate:
                    currentWorkSet = unitOfWork.RealEstates.GetAll();
                    break;
                case SearchMode.SearchAll:
                    currentWorkSet = unitOfWork.Clients.GetAll();
                    currentWorkSet = currentWorkSet.Union( unitOfWork.Companies.GetAll() );
                    currentWorkSet = currentWorkSet.Union( unitOfWork.Vehiecles.GetAll() );
                    currentWorkSet = currentWorkSet.Union( unitOfWork.RealEstates.GetAll() );
                    break;
                case SearchMode.None:
                default:
                    break;
            }

            WrappedObject.SearchResults = new ObservableCollection<IDataModel>( currentWorkSet );

        }

        private bool ShouldShowItem( object item )
        {
            if ( string.IsNullOrWhiteSpace( WrappedObject.SearchText ) )
            {
                return true;
            }

            if ( item is FiscalEntityDataModel fiscalEntity )
            {
                string searchText = WrappedObject.SearchText;
                bool result = false;
                
                if ( fiscalEntity.CommonId != null )
                {
                    result = fiscalEntity.CommonId.Contains( searchText , StringComparison.InvariantCultureIgnoreCase );
                }

                if ( fiscalEntity.FiscalNumber != null )
                {
                    result = result || fiscalEntity.FiscalNumber.Contains( searchText , StringComparison.InvariantCultureIgnoreCase );
                }

                if ( fiscalEntity.CustomerGroup != null && !string.IsNullOrWhiteSpace( fiscalEntity.CustomerGroup.Name ) )
                {
                    result = result || fiscalEntity.CustomerGroup.Name.Contains( searchText , StringComparison.InvariantCultureIgnoreCase );
                }
                
                return result;
            }
            else
            {
                return false;
            }
        }
    }

    public enum SearchMode
    {
        None,
        SearchAll,
        SearchFiscalEntity,
        SearchClient,
        SearchCompany,
        SearchVehiecle,
        SearchRealEstate
    }
}
