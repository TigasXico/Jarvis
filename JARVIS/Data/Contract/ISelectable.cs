using System.ComponentModel;

namespace Jarvis.Data.Contract
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
