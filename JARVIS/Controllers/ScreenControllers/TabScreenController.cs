using System.Collections.Generic;
using Jarvis.Controllers.Contract;
using Jarvis.Data.DataModels;

namespace Jarvis.Controllers.ScreenControllers
{
    public class TabScreenController : ObjectWrapper<IDataModelScreenController<FiscalEntityDataModel>>
    {
        private readonly ICollection<TabScreenController> holderOfTabs;

        private bool displaySaveButtons;
        public bool DisplaySaveButtons
        {
            get => displaySaveButtons;
            set => SetProperty( ref displaySaveButtons , value );
        }

        public string TabName => WrappedObject.ModelCommonId;

        private IDataModelController<FiscalEntityDataModel> ControllerForEntity => WrappedObject.DataModelController;

        public TabScreenController( IDataModelScreenController<FiscalEntityDataModel> tabbedModelWrapper , ICollection<TabScreenController> entitiesBeingEdited ) : base( tabbedModelWrapper )
        {
            holderOfTabs = entitiesBeingEdited;
        }

        internal void RemoveFromScreen()
        {
            holderOfTabs.Remove( this );
        }
    }
}




