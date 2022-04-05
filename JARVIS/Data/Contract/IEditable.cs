namespace Jarvis.Data.Contract
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