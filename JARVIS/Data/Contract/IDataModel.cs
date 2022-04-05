namespace Jarvis.Data.Contract
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
