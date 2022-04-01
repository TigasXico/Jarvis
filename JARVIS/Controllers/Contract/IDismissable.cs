using System.Windows.Input;

namespace Jarvis.Interfaces
{
    public interface IDismissable
    {
        ICommand OkCommand
        {
            get;
        }

        ICommand CancelCommand
        {
            get;
        }

        bool DisplayControlButtons
        {
            get;
        }
    }
}
