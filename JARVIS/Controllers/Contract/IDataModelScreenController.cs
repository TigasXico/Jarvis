using System.ComponentModel;
using Jarvis.Data.Contract;

namespace Jarvis.Controllers.Contract
{
    public interface IDataModelScreenController<T> :  INotifyPropertyChanged where T : IDataModel
    {
        IUpdatableDataModelController<T> DataModelController
        {
            get;
        }

        T Entity
        {
            get;
        }

        string ModelCommonId
        {
            get;
        }
    }
}
