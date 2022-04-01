namespace Jarvis.Interfaces
{
    public interface IDataModel : IDatabaseModel, ISelectable, IEditable
    {
        /// <summary>
        /// The common identification for this Model
        /// </summary>
        string CommonId
        {
            get;
        }
    }
}
