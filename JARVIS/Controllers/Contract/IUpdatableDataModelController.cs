using Jarvis.Data.Contract;

namespace Jarvis.Controllers.Contract
{
    public interface IUpdatableDataModelController<T> : IDataModelController<T> where T : IDataModel
    {
        OperationResult UpdateEntityInfo( bool isSilentUpdate = false );
    }
}