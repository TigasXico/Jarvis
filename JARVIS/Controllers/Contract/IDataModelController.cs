using Jarvis.Data.Contract;
using Jarvis.Data.Contract.Repositories;

namespace Jarvis.Controllers.Contract
{
    public interface IDataModelController<T> where T : IDataModel
    {
        IUnitOfWork UnitOfWork
        {
            get;
            set;
        }

        T Model
        {
            get;
        }

        bool PersistChanges();

        bool DeleteEntity();
    }

    public enum OperationResult
    {
        Default,
        Success,
        WrongCredentials,
        Failed
    }
}
