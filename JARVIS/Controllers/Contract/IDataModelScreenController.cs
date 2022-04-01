using System.ComponentModel;

using Jarvis.Controllers.ModelControllers;

namespace Jarvis.Interfaces
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
