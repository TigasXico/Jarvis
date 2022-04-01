
using System.ComponentModel.DataAnnotations.Schema;

using Jarvis.Interfaces;
using Jarvis.Utils.HelperClasses;

namespace Jarvis.DataModels
{
    public abstract class BaseDataModel : PropertyChangedRaiser, IDataModel
    {
        private int Id;
        public int ID
        {
            get => Id;
            set => SetProperty( ref Id , value );
        }

        /// <summary>
        /// The common/human-friendly ID for a model
        /// </summary>
        public abstract string CommonId
        {
            get;
        }

        public object updatingCollectionsLock = new object();

        private bool isSelected;
        [NotMapped]
        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty( ref isSelected , value );
        }

        private bool isNew;
        [NotMapped]
        public bool IsNew
        {
            get => isNew;
            set => SetProperty( ref isNew , value );
        }

        private bool isDirty;
        [NotMapped]
        public bool IsDirty
        {
            get => isDirty;
            set => SetProperty( ref isDirty , value );
        }

        [NotMapped]
        public abstract string ShortDescriptor
        {
            get;
        }
    }
}
