using System.Windows.Input;

namespace Jarvis.Controllers.Contract
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
