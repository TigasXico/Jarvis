namespace Jarvis.Interfaces
{
    public interface IEditable
    {
        bool IsNew
        {
            get;
            set;
        }

        bool IsDirty
        {
            get;
            set;
        }
    }
}