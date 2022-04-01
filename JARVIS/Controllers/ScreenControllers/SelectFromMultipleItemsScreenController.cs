using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

using GalaSoft.MvvmLight.CommandWpf;

using Jarvis.Interfaces;
using Jarvis.Services;
using Jarvis.Utils.HelperClasses;

namespace Jarvis.Controllers.ScreenControllers
{
    public class SelectFromMultipleItemsScreenController<T> : BaseScreenController where T: IDataModel
    {
        private ICollectionView searchResultsView;
        private readonly IEnumerable<T> existingItems;

        private string searchLabel;
        public string SearchLabel
        {
            get => searchLabel;
            set => SetProperty(ref searchLabel , value);
        }

        private string searchValue;
        public string SearchValue
        {
            get => searchValue;
            set
            {
                SetProperty( ref searchValue , value );
                searchResultsView.Refresh();
            }
        }

        private T selectedItem;
        public T SelectedItem
        {
            get => selectedItem;
            set => SetProperty( ref selectedItem , value );
        }

        private bool itemWasSelected;
        public bool ItemWasSelected
        {
            get => itemWasSelected;
            private set => SetProperty( ref itemWasSelected , value );
        }

        private ObservableCollection<T> searchMatches;
        public ObservableCollection<T> SearchMatches
        {
            get => searchMatches;
            set
            {
                if ( SetProperty( ref searchMatches , value ) )
                {
                    searchResultsView = CollectionViewSource.GetDefaultView( searchMatches );
                    searchResultsView.Filter = FilterItem;
                }
            }
        }

        public SelectFromMultipleItemsScreenController( IEnumerable<T> existingItems)
        {
            OkCommand = new RelayCommand( OkAction , CanExecuteOkAction );
            CancelCommand = new RelayCommand( CancelAction );

            DisplayControlButtons = true;

            this.existingItems = existingItems;

            SearchMatches = new ObservableCollection<T>( existingItems );

            OkCommand = new RelayCommand(OkAction , CanExecuteOkAction);

            CancelCommand = new RelayCommand(CancelAction);
        }

        private bool FilterItem( object item )
        {
            if ( string.IsNullOrWhiteSpace( SearchValue ) )
            {
                return true;
            }

            if ( item is IDataModel dataModel )
            {
                return dataModel.CommonId.Contains( SearchValue , StringComparison.InvariantCultureIgnoreCase );
            }
            else
            {
                return false;
            }
        }

        private void OkAction()
        {
            //If no item was selected, check if no selection was accidental
            if ( EqualityComparer<T>.Default.Equals( SelectedItem , default ) )
            {
                // Item will have existing value or default
                SelectedItem = existingItems.SingleOrDefault( item => item.CommonId.Equals( SearchValue , StringComparison.InvariantCultureIgnoreCase ));
            }

            //if value is default, then no item was selected
            //otherwise, there is a selected item to be used
            ItemWasSelected = ! EqualityComparer<T>.Default.Equals( SelectedItem , default );

            WindowService.CloseWindowOfViewModel( this , true );
        }

        private bool CanExecuteOkAction()
        {
            return !string.IsNullOrWhiteSpace( SearchLabel );
        }

        private void CancelAction()
        {
            WindowService.CloseWindowOfViewModel( this , false );
        }
    }
}
