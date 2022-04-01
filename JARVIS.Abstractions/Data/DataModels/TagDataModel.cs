

using System.Collections.Generic;

namespace Jarvis.DataModels
{
    public class TagDataModel : BaseDataModel
    {
        /// <summary>
        /// The common identification for Tags
        /// </summary>
        public override string CommonId => Name;

        public override string ShortDescriptor => throw new System.NotImplementedException();

        private string name;
        /// <summary>
        /// The tag name
        /// </summary>
        public string Name
        {
            get => name;
            set => SetProperty( ref name , value );
        }

        private List<FiscalEntityDataModel> fiscalEntities;
        /// <summary>
        /// List of entities that have this Tag
        /// </summary>
        public List<FiscalEntityDataModel> FiscalEntities
        {
            get => fiscalEntities;
            set => SetProperty( ref fiscalEntities , value );
        }

        private List<VehiecleDataModel> vehiecles;
        /// <summary>
        /// List of vehiecles that have this Tag
        /// </summary>
        public List<VehiecleDataModel> Vehiecles
        {
            get => vehiecles;
            set => SetProperty( ref vehiecles , value );
        }

        private List<RealEstateDataModel> realEstates;
        /// <summary>
        /// List of real estates that have this Tag
        /// </summary>
        public List<RealEstateDataModel> RealEstates
        {
            get => realEstates;
            set => SetProperty( ref realEstates , value );
        }
    }
}
