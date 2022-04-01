

using System.Collections.Generic;

namespace Jarvis.Data.DataModels
{
    public class CustomerGroupDataModel : NamedDataModel
    {
        /// <summary>
        /// The common identification for Customer Groups
        /// </summary>
        public override string CommonId => Name;

        public override string ShortDescriptor => $"{Name}";

        private List<FiscalEntityDataModel> fiscalEntities;
        /// <summary>
        /// List of entities that are in this customer group
        /// </summary>
        public virtual List<FiscalEntityDataModel> FiscalEntities
        {
            get => fiscalEntities;
            set => SetProperty( ref fiscalEntities , value );
        }

        private int currentIdCount;
        public int CurrentIdCount
        {
            get => currentIdCount;
            set => SetProperty(ref currentIdCount , value);
        }

        public CustomerGroupDataModel()
        {
        }
    }
}
