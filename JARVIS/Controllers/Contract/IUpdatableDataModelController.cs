using Jarvis.Interfaces;

namespace Jarvis.Controllers.ModelControllers
{
    public interface IUpdatableDataModelController<T> : IDataModelController<T> where T : IDataModel
    {
        OperationResult UpdateEntityInfo( bool isSilentUpdate = false );
    }
}