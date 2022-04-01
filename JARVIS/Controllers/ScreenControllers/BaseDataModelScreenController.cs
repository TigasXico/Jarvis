using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using Jarvis.Controllers.ModelControllers;
using Jarvis.Interfaces;

namespace Jarvis.Controllers.ScreenControllers
{
    public abstract class DataModelScreenController<T> : BaseDataModelScreenController<T>, IDataModelScreenController<T> where T : IDataModel
    {
        public DataModelScreenController( IUpdatableDataModelController<T> dataModelController ) : base( dataModelController.Model )
        {
            DataModelController = dataModelController;
        }

        public string ModelCommonId => WrappedObject.CommonId;

        public IUpdatableDataModelController<T> DataModelController
        {
            get;
            private set;
        }

        public T Entity => WrappedObject;
    }

    public abstract class BaseDataModelScreenController<T> : ObjectWrapper<T>
    {
        private bool displayControlButtons;
        public bool DisplayControlButtons
        {
            get => displayControlButtons;
            set => SetProperty( ref displayControlButtons , value );
        }

        private bool enableControls;
        public bool EnableControls
        {
            get => enableControls;
            set => SetProperty( ref enableControls , value );
        }

        private bool readOnlyControlls;
        public bool ReadOnlyControlls
        {
            get => readOnlyControlls;
            set => SetProperty( ref readOnlyControlls , value );
        }

        public BaseDataModelScreenController( T dataModel ) : base( dataModel )
        {
        }
    }

    public abstract class ObjectWrapper<T> : DynamicObject, INotifyPropertyChanged
    {
        private T wrappedObject;

        public T WrappedObject
        {
            get => wrappedObject;
            private set => SetProperty( ref wrappedObject , value );
        }


        protected bool WrappedModelRaisesPropertyChanged
        {
            get; set;
        }

        public ObjectWrapper( T entityToBeWrapped )
        {
            WrappedObject = entityToBeWrapped;

            // Make controller raise property changed of wrapped object
            if ( WrappedObject is INotifyPropertyChanged raiserObject )
            {
                WrappedModelRaisesPropertyChanged = true;
                raiserObject.PropertyChanged += ( sender , args ) =>
                    RaisePropertyChanged( args.PropertyName );
            }
        }

        public override bool TryGetMember( GetMemberBinder binder , out object result )
        {
            string propertyName = binder.Name;
            PropertyInfo property =
              WrappedObject?.GetType().GetProperty( propertyName );

            if ( property == null || property.CanRead == false )
            {
                result = null;
                return false;
            }

            result = property.GetValue( WrappedObject , null );
            return true;
        }

        public override bool TrySetMember( SetMemberBinder binder , object value )
        {
            string propertyName = binder.Name;
            PropertyInfo property =
              WrappedObject?.GetType().GetProperty( propertyName );

            if ( property == null || property.CanWrite == false )
            {
                return false;
            }

            property.SetValue( WrappedObject , value , null );

            if ( !WrappedModelRaisesPropertyChanged )
            {
                RaisePropertyChanged( property.Name );
            }
            return true;
        }

        #region Raise Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged( [CallerMemberName] string propertyName = null )
        {
            PropertyChanged?.Invoke( this , new PropertyChangedEventArgs( propertyName ) );
        }

        protected virtual bool SetProperty<T1>( ref T1 storedValue , T1 newValue , [CallerMemberName] string propertyName = null )
        {
            if ( EqualityComparer<T1>.Default.Equals( storedValue , newValue ) )
            {
                return false;
            }

            storedValue = newValue;

            RaisePropertyChanged( propertyName );

            return true;
        }

        #endregion
    }

    public abstract class BaseScreenController : PropertyRaiser
    {
        private bool displayControlButtons;
        public bool DisplayControlButtons
        {
            get => displayControlButtons;
            set => SetProperty( ref displayControlButtons , value );
        }

        private bool enableControls;
        public bool EnableControls
        {
            get => enableControls;
            set => SetProperty( ref enableControls , value );
        }

        private bool readOnlyControlls;
        public bool ReadOnlyControlls
        {
            get => readOnlyControlls;
            set => SetProperty( ref readOnlyControlls , value );
        }

        public ICommand OkCommand
        {
            get;
            set;
        }

        public ICommand CancelCommand
        {
            get;
            set;
        }
    }

    public abstract class PropertyRaiser : INotifyPropertyChanged
    {
        #region Raise Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged( [CallerMemberName] string propertyName = null )
        {
            PropertyChanged?.Invoke( this , new PropertyChangedEventArgs( propertyName ) );
        }

        protected virtual bool SetProperty<T>( ref T storedValue , T newValue , [CallerMemberName] string propertyName = null )
        {
            if ( EqualityComparer<T>.Default.Equals( storedValue , newValue ) )
            {
                return false;
            }

            storedValue = newValue;

            RaisePropertyChanged( propertyName );

            return true;
        }

        #endregion
    }

}
