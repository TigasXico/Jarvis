using Jarvis.DataAccess.Repositories;
using Jarvis.DataAcess.Contract;
using Jarvis.Interfaces;

namespace Jarvis.Controllers.ModelControllers
{
    public abstract class BaseDataModelController<T> : IDataModelController<T> where T : IDataModel
    {
        private IUnitOfWork unitOfWork;
        public IUnitOfWork UnitOfWork
        {
            get
            {
                if ( unitOfWork == null )
                {
                    unitOfWork = new UnitOfWork();
                }

                return unitOfWork;
            }

            set => unitOfWork = value;
        }

        public T Model
        {
            get;
            protected set;
        }

        public abstract bool PersistChanges();

        public abstract bool DeleteEntity();

        public BaseDataModelController( T model )
        {
            Model = model;
        }
    }

    public abstract class UpdatableDataModelController<T> : BaseDataModelController<T> , IUpdatableDataModelController<T> where T:IDataModel
    {
        public abstract OperationResult UpdateEntityInfo( bool isSilentUpdate = false );

        public UpdatableDataModelController(T updatableModel) : base(updatableModel)
        {

        }
    }
}
