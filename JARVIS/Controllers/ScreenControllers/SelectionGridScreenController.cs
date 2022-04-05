using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Jarvis.Data.Contract;
using Jarvis.Utils.HelperClasses;

namespace Jarvis.Controllers.ScreenControllers
{
    public class SelectionGridScreenController : PropertyChangedRaiser
    {
        private ICollectionView searchResultsView;

        private bool isAllSelectedChanging;

        private string searchText;
        public string SearchText
        {
            get => searchText;
            set
            {
                SetProperty( ref searchText , value );
                UpdateSearchResults();
            }
        }

        private ObservableCollection<IDataModel> searchResults;
        public ObservableCollection<IDataModel> SearchResults
        {
            get => searchResults;
            set
            {
                if ( SetProperty( ref searchResults , value ) )
                {
                    foreach ( ISelectable entry in searchResults )
                    {
                        entry.PropertyChanged += ItemSelectedStateChanged;
                    }

                    searchResultsView = CollectionViewSource.GetDefaultView( SearchResults );
                    searchResultsView.Filter = Filter;
                }
            }
        }

        private bool allowMultipleSelection;
        public bool AllowMultipleSelection
        {
            get => allowMultipleSelection;
            set => SetProperty( ref allowMultipleSelection , value );
        }

        private bool? selectAllState;
        public bool? SelectAllState
        {
            get => selectAllState;
            set
            {
                if ( SetProperty( ref selectAllState , value ) )
                {
                    SelectAllStateChanged();
                }
            }
        }

        private Predicate<object> filter;
        public Predicate<object> Filter
        {
            get => filter;
            set
            {
                if ( SetProperty( ref filter , value ) )
                {
                    searchResultsView.Filter = filter;
                }
            }
        }

        public SelectionGridScreenController( bool allowMultipleSelection = false )
        {
            SearchResults = new ObservableCollection<IDataModel>();

            AllowMultipleSelection = allowMultipleSelection;

            SelectAllState = false;
        }

        private void ItemSelectedStateChanged( object sender , PropertyChangedEventArgs e )
        {
            if ( e.PropertyName == nameof( ISelectable.IsSelected ) )
            {
                RecheckAllSelected();
            }
        }

        private void SelectAllStateChanged()
        {
            // Has this change been caused by some other change?
            // return so we don't mess things up
            if ( isAllSelectedChanging )
            {
                return;
            }

            try
            {
                isAllSelectedChanging = true;

                foreach ( ISelectable entry in SearchResults )
                {
                    if ( searchResultsView.Contains( entry ) )
                    {
                        entry.IsSelected = SelectAllState.HasValue && SelectAllState.Value;
                    }
                }
            }
            finally
            {
                isAllSelectedChanging = false;
            }
        }

        private void RecheckAllSelected()
        {
            // Has this change been caused by some other change?
            // return so we don't mess things up
            if ( isAllSelectedChanging )
            {
                return;
            }

            try
            {
                isAllSelectedChanging = true;

                if ( SearchResults.All( e => e.IsSelected ) )
                {
                    SelectAllState = true;
                }
                else if ( SearchResults.All( e => !e.IsSelected ) )
                {
                    SelectAllState = false;
                }
                else
                {
                    SelectAllState = null;
                }
            }
            finally
            {
                isAllSelectedChanging = false;
            }
        }

        private void UpdateSearchResults()
        {
            searchResultsView.Refresh();
            RecheckAllSelected();
        }

        public List<IDataModel> GetSelectedItems()
        {
            return SearchResults.Where( item => item.IsSelected ).ToList();
        }
    }

}
