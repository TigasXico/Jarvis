using System.ComponentModel;

namespace Jarvis.Interfaces
{
    public interface ISelectable : INotifyPropertyChanged
    {
        bool IsSelected
        {
            get;
            set;
        }
    }
}
