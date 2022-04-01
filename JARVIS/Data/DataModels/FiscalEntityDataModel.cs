using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

using Jarvis.Interfaces;

namespace Jarvis.Data.DataModels
{
    public abstract class FiscalEntityDataModel : NamedDataModel, IFiscalEntity
    {
        public override string CommonId
        {
            get
            {
                if ( string.IsNullOrWhiteSpace( Name ) )
                {
                    return FiscalNumber + " - (Nome ainda não obtido) ";
                }
                else
                {
                    return Name;
                }
            }
        }

        public override string ShortDescriptor => ToString();

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

        private string socialSecurityNumber;
        /// <summary>
        /// Fiscal Entity identifying number for the Social Security services
        /// (NISS)
        /// </summary>
        public string SocialSecurityNumber
        {
            get => fiscalNumber;
            set => SetProperty( ref socialSecurityNumber , value );
        }

        private string socialSecurityPassword;
        /// <summary>
        /// Fiscal Entity Password to access the Social Security services
        /// </summary>
        public string SocialSecurityPassword
        {
            get => fiscalPassword;
            set => SetProperty( ref socialSecurityPassword , value );
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

        private CustomerGroupDataModel customerGroup;
        public virtual CustomerGroupDataModel CustomerGroup
        {
            get => customerGroup;
            set
            {
                SetProperty( ref customerGroup , value );
                RaisePropertyChanged( nameof( CustomerGroupFullId ) );
            }
        }

        private int? customerGroupId;
        public int? CustomerGroupId
        {
            get => customerGroupId;
            set => SetProperty( ref customerGroupId , value );
        }

        private int? idOnCustomerGroup;
        public int? IdOnCustomerGroup
        {
            get => idOnCustomerGroup;
            set
            {
                SetProperty( ref idOnCustomerGroup , value );
                RaisePropertyChanged( nameof( CustomerGroupFullId ) );
            }
        }

        [NotMapped]
        public string CustomerGroupFullId
        {
            get
            {
                if ( CustomerGroup != null && !string.IsNullOrWhiteSpace( CustomerGroup.Name ) && IdOnCustomerGroup != null )
                {
                    return $"{CustomerGroup.Name} - {IdOnCustomerGroup}";
                }
                else
                {
                    return string.Empty;
                }
            }
        }


        private ObservableCollection<ContactDataModel> contacts;
        /// <summary>
        /// The registered contacts of this Fiscal Entity
        /// </summary>
        public virtual ObservableCollection<ContactDataModel> Contacts
        {
            get => contacts;
            set => SetProperty( ref contacts , value );
        }

        private ObservableCollection<VehiecleDataModel> vehiecles;
        /// <summary>
        /// The ObservableCollection of vehiecles assigned to this Fiscal Entity
        /// </summary>
        public virtual ObservableCollection<VehiecleDataModel> Vehiecles
        {
            get => vehiecles;
            set => SetProperty( ref vehiecles , value );
        }

        private ObservableCollection<RealEstateDataModel> realEstates;
        /// <summary>
        /// The ObservableCollection of vehiecles assigned to this Fiscal Entity
        /// </summary>
        public virtual ObservableCollection<RealEstateDataModel> RealEstates
        {
            get => realEstates;
            set => SetProperty( ref realEstates , value );
        }

        private ObservableCollection<ImiChargeNotesDataModel> imiChargeNotes;
        public virtual ObservableCollection<ImiChargeNotesDataModel> ImiChargeNotes
        {
            get => imiChargeNotes;
            set => SetProperty( ref imiChargeNotes , value );
        }

        private ObservableCollection<TransactionDataModel> transactions;
        public virtual ObservableCollection<TransactionDataModel> Transactions
        {
            get => transactions;
            set => SetProperty( ref transactions, value );
        }

        private decimal currentBalance;
        public decimal CurrentBalance
        {
            get => currentBalance;
            set => SetProperty(ref currentBalance , value);
        }

        private FiscalEntityStatus currentStatus;
        public FiscalEntityStatus CurrentStatus
        {
            get => currentStatus;
            set => SetProperty( ref currentStatus , value );
        }
        [NotMapped]
        public string CurrentStatusString
        {
            get
            {
                switch ( CurrentStatus )
                {
                    case FiscalEntityStatus.Unknown:
                        return "Desconhecido";
                    case FiscalEntityStatus.Updated:
                        return "Atualizado";
                    case FiscalEntityStatus.NotUpdated:
                        return "Pronto a atualizar";
                    case FiscalEntityStatus.WrongCredentials:
                        return "Credenciais erradas";
                    case FiscalEntityStatus.InvalidFiscalNumber:
                        return "NIF inválido";
                    case FiscalEntityStatus.MissingInformation:
                        return "Falta informação";
                    default:
                        return default;
                }
            }
        }

        public override string ToString()
        {
            StringBuilder description = new StringBuilder();
            description.AppendLine( $"Nome completo: {Name}" );
            description.AppendLine( $"NIF: {FiscalNumber}" );
            return description.ToString();
        }

    }

    public enum FiscalEntityStatus
    {
        Unknown = 0,
        Updated = 1,
        NotUpdated = 2,
        WrongCredentials = 3,
        InvalidFiscalNumber = 4,
        MissingInformation = 5
    }
}

