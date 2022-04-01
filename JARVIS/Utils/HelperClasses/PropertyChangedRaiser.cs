using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Jarvis.Utils.HelperClasses

{
    public abstract class PropertyChangedRaiser : INotifyPropertyChanged
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
