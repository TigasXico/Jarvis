using System.Collections.Generic;
using System.Text;

using Jarvis.Interfaces;

namespace Jarvis.DataModels
{
    public abstract class FiscalEntityDataModel : BaseDataModel , IFiscalEntity
    {
        public override string CommonId => $"{FiscalNumber} - {Name}";

        public override string ShortDescriptor => ToString();

        private string name;
        /// <summary>
        /// The Fiscal Entity common/human-friendly designation
        /// </summary>
        public string Name
        {
            get => name;
            set => SetProperty( ref name , value );
        }

        private string fiscalNumber;
        /// <summary>
        /// Fiscal Entity identifying number for the Finacial Services
        /// (NIF)
        /// </summary>
        public string FiscalNumber
        {
            get => fiscalNumber;
            set => SetProperty( ref fiscalNumber , value );
        }

        private string fiscalPassword;
        /// <summary>
        /// Fiscal Entity Password to access the Finacial Services
        /// </summary>
        public string FiscalPassword
        {
            get => fiscalPassword;
            set => SetProperty( ref fiscalPassword , value );
        }

        private string fiscalAddress;
        /// <summary>
        /// The Fiscal Entity registered fiscal address
        /// </summary>
        public string FiscalAddress
        {
            get => fiscalAddress;
            set => SetProperty( ref fiscalAddress , value );
        }

        private string fiscalAddressAdditionalInfo;
        /// <summary>
        /// The Fiscal Entity fiscal address additional info
        /// </summary>
        public string FiscalAddressAdditionalInfo
        {
            get => fiscalAddressAdditionalInfo;
            set => SetProperty( ref fiscalAddressAdditionalInfo , value );
        }

        private string fiscalAddressZipCode;
        /// <summary>
        /// The Fiscal Entity registered fiscal address zip code
        /// </summary>
        public string FiscalAddressZipCode
        {
            get => fiscalAddressZipCode;
            set => SetProperty( ref fiscalAddressZipCode , value );
        }

        private string financialServicesRepartition;
        /// <summary>
        /// The Financial Services repartition responsible for this Fiscal Entity
        /// </summary>
        public string FinancialServicesRepartition
        {
            get => financialServicesRepartition;
            set => SetProperty( ref financialServicesRepartition , value );
        }

        private string notes;
        /// <summary>
        /// Notes associated with this Fiscal Entity
        /// </summary>
        public string Notes
        {
            get => notes;
            set => SetProperty( ref notes , value );
        }

        private List<TagDataModel> tags;
        /// <summary>
        /// The tags of this Fiscal Entity
        /// </summary>
        public List<TagDataModel> Tags
        {
            get => tags;
            set => SetProperty( ref tags , value );
        }

        private List<ContactDataModel> contacts;
        /// <summary>
        /// The registered contacts of this Fiscal Entity
        /// </summary>
        public List<ContactDataModel> Contacts
        {
            get => contacts;
            set => SetProperty( ref contacts , value );
        }

        private List<VehiecleDataModel> vehiecles;
        /// <summary>
        /// The list of vehiecles assigned to this Fiscal Entity
        /// </summary>
        public List<VehiecleDataModel> Vehiecles
        {
            get => vehiecles;
            set => SetProperty( ref vehiecles , value );
        }

        private List<RealEstateDataModel> realEstates;
        /// <summary>
        /// The list of vehiecles assigned to this Fiscal Entity
        /// </summary>
        public List<RealEstateDataModel> RealEstates
        {
            get => realEstates;
            set => SetProperty( ref realEstates , value );
        }

        public override string ToString()
        {
            StringBuilder description = new StringBuilder();
            description.AppendLine( $"Nome completo: {Name}" );
            description.AppendLine( $"NIF: {FiscalNumber}" );
            description.AppendLine( $"Morada Fiscal: {FiscalAddress} - Cód. Postal: {FiscalAddressZipCode}" );
            description.Append( $"Rep. Finanças: {FinancialServicesRepartition}" );
            return description.ToString();
        }

    }
}

