using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Windows.Data;
using Jarvis.Controllers.Contract;
using Jarvis.Controllers.ScreenControllers;
using Jarvis.Data.Contract;
using Jarvis.Data.Contract.Repositories;
using Jarvis.Data.DataAccess.Extractors;
using Jarvis.Data.DataAccess.Scraping;
using Jarvis.Data.DataModels;
using Jarvis.Services;
using Jarvis.Utils.HelperClasses;

using log4net;

namespace Jarvis.Controllers.ModelControllers
{
    public enum FiscalEnityType
    {
        Company,
        Client
    }

    public enum FiscalEntityGenerationResult
    {
        Default,
        InvalidFiscalNumber,
        CannotResolveFiscalNumberToEntityType,
        NoError
    }
    
    public abstract class FiscalEntityController : UpdatableDataModelController<FiscalEntityDataModel> , IDataModelController<FiscalEntityDataModel>
    {
        #region Private variables

        private readonly ILog logger = LogManager.GetLogger( typeof( FiscalEntityController ) );

        private readonly IWebScraper<FiscalEntityDataModel> financesWebScraper;

        private readonly BackgroundWorker Worker;

        #endregion

        #region Properties

        #region Configurations related to client

        private const string InvalidEntitiesTemplatePathConfigKey = "InvalidEntitiesTemplatePath";
        private static string InvalidEntitiesTemplatePath => ConfigurationManager.AppSettings.Get( InvalidEntitiesTemplatePathConfigKey );

        #endregion

        #endregion

        #region Validation related constants

        private static readonly string[] singularpersonfiscalNumberPossibleStarts =
        {
            "45",   // pessoa singular não residente,
            "1",    // pessoa singular
            "2",    // pessoa singular
            "3",    // pessoa singular
        };

        private static readonly string[] collectivepersonfiscalNumberPossibleStarts =
        {
            "71",   // pessoa colectiva não residente
            "5",    // pessoa colectiva
        };

        private static readonly string[] otherFiscalNumberPossibleStarts =
        {
            "70",   // herança indivisa,
            "72",   // fundos de investimento,
            "77",   // atribuição oficiosa,
            "79",   // regime excepcional,
            "90",   // condominios e sociedades irregulares,
            "91",   // condominios e sociedades irregulares,
            "98",   // não residentes,
            "99",   // sociedades civis,
            "6",    // administração pública
            "8",    // empresário em nome individual (extinto)
        };

        #endregion

        #region Constructors

        public FiscalEntityController( BackgroundWorker worker , IUnitOfWork providedUnitOfWork = null ) : base(null)
        {
            if ( worker != null )
            {
                Worker = worker;
            }

            financesWebScraper = new FinancesWebScraperFactory().GetScraper();

            if ( providedUnitOfWork != null )
            {
                UnitOfWork = providedUnitOfWork;
            }
        }

        #endregion

        #region Static Methods

        public static bool IsEntityInfoValid( IFiscalEntity entity )
        {
            return entity != null && !string.IsNullOrWhiteSpace( entity.FiscalNumber ) && !string.IsNullOrWhiteSpace( entity.FiscalPassword );
        }

        public static bool IsFiscalNumberValid( string fiscalNumber )
        {
            if ( string.IsNullOrWhiteSpace( fiscalNumber ) )
            {
                return false;
            }

            // algoritmo de validação do NIF de acordo com
            // http://pt.wikipedia.org/wiki/N%C3%BAmero_de_identifica%C3%A7%C3%A3o_fiscal

            if ( fiscalNumber.Length != 9 )
            {
                return false;
            }

            if (
                !fiscalNumber.StartsWith( singularpersonfiscalNumberPossibleStarts ) &&
                !fiscalNumber.StartsWith( collectivepersonfiscalNumberPossibleStarts ) &&
                !fiscalNumber.StartsWith( otherFiscalNumberPossibleStarts )
                )
            {
                return false;
            }

            var fiscalNumberAsNumberArray = fiscalNumber.ToCharArray().Select( c => Convert.ToInt64( c.ToString() ) ).ToArray();

            long sumNumber = 0;

            for ( var i = 0 ; i < fiscalNumber.Length - 1 ; i++ )
            {
                sumNumber += (fiscalNumberAsNumberArray[i] * (9 - i));
            }

            var controlSum = (sumNumber % 11);


            if ( controlSum == 0 || controlSum == 1 )
            {
                controlSum = 0;
            }
            else
            {
                controlSum = 11 - controlSum;
            }
            

            return controlSum == fiscalNumberAsNumberArray[fiscalNumber.Length - 1];
        }

        public static bool IsFiscalNumberOfType( string fiscalNumber , FiscalEnityType targetType )
        {
            if ( string.IsNullOrEmpty( fiscalNumber ) )
            {
                return false;
            }

            var result = false;

            switch ( targetType )
            {
                case FiscalEnityType.Company:
                    result = fiscalNumber.StartsWith( collectivepersonfiscalNumberPossibleStarts );
                    break;
                case FiscalEnityType.Client:
                    result = fiscalNumber.StartsWith( singularpersonfiscalNumberPossibleStarts );
                    break;
            }

            return result;
        }

        public static FiscalEntityGenerationResult ResolveFiscalInfoToEntity( string fiscalNumber , string fiscalPassword , out FiscalEntityDataModel fiscalEntity )
        {
            fiscalEntity = null;

            if ( !IsFiscalNumberValid( fiscalNumber ) )
            {
                return FiscalEntityGenerationResult.InvalidFiscalNumber;
            }

            //Determine fiscal entity type
            if ( IsFiscalNumberOfType( fiscalNumber , FiscalEnityType.Client ) )
            {
                fiscalEntity = new ClientDataModel();
            }
            else if ( IsFiscalNumberOfType( fiscalNumber , FiscalEnityType.Company ) )
            {
                fiscalEntity = new CompanyDataModel();
            }
            else
            {
                return FiscalEntityGenerationResult.CannotResolveFiscalNumberToEntityType;
            }

            fiscalEntity.FiscalNumber = fiscalNumber;
            fiscalEntity.FiscalPassword = fiscalPassword;

            return FiscalEntityGenerationResult.NoError;
        }

        /// <summary>
        /// Presents the user with a screen to select a new Customer Group. Returns the selected group and/or if the action was canceled.
        /// </summary>
        /// <param name="existingCustomerGroups"> The list of existing customer groups, for display </param>
        /// <param name="selectedCustomerGroup"> The selected customer group. Can be null if user did not specify any text input or if the action was cancelled </param>
        /// <returns> If result is to be considered: if action was cancelled, then false. Otherwise, true </returns>
        public static bool GetCustomerGroupSelection( IEnumerable<CustomerGroupDataModel> existingCustomerGroups , out CustomerGroupDataModel selectedCustomerGroup )
        {
            //Open window to update customer group
            var screenController =
                new SelectCustomerGroupScreenController( existingCustomerGroups )
                {
                    DisplayControlButtons = true ,
                    EnableControls = true ,
                    ReadOnlyControlls = false ,
                    SearchLabel = "Grupo :"
                };

            if ( WindowService.ShowWindowForController( screenController , "Atualizar grupo da entidade" ) )
            {
                selectedCustomerGroup = ExtractResult( screenController );

                if ( selectedCustomerGroup?.CurrentIdCount == 0 )
                {
                    selectedCustomerGroup.CurrentIdCount = 1;
                }

                return true;
            }
            else
            {
                selectedCustomerGroup = null;
                return false;
            }
        }

        protected static T ExtractResult<T>( SelectFromMultipleItemsScreenController<T> screenController ) where T : NamedDataModel, new()
        {
            //Item was selected?
            if ( screenController.ItemWasSelected )
            {
                //selected item is considered the aggregate
                return screenController.SelectedItem;
            }
            else if ( !string.IsNullOrWhiteSpace( screenController.SearchValue ) )
            {
                //no item selected, but name given -> generate new aggregae with provided name
                return new T()
                {
                    Name = screenController.SearchValue
                };
            }
            else
            {
                //no selected item and no name given -> set aggregate to null
                return default;
            }
        }

        #endregion

        #region Entity handling

        private void EnsureCollectionsAreInitialized()
        {
            if ( Model.Contacts == null )
            {
                Model.Contacts = new ObservableCollection<ContactDataModel>();
            }

            if ( Model.Vehiecles == null )
            {
                Model.Vehiecles = new ObservableCollection<VehiecleDataModel>();
            }

            if ( Model.RealEstates == null )
            {
                Model.RealEstates = new ObservableCollection<RealEstateDataModel>();
            }

            if (Model.ImiChargeNotes == null)
            {
                Model.ImiChargeNotes = new ObservableCollection<ImiChargeNotesDataModel>();
            }
        }

        protected void PrepareCollectionsForUpdate()
        {
            EnsureCollectionsAreInitialized();

            BindingOperations.EnableCollectionSynchronization( Model.Contacts , Model.updatingCollectionsLock );
            BindingOperations.EnableCollectionSynchronization( Model.Vehiecles , Model.updatingCollectionsLock );
            BindingOperations.EnableCollectionSynchronization( Model.RealEstates , Model.updatingCollectionsLock );
            BindingOperations.EnableCollectionSynchronization( Model.ImiChargeNotes, Model.updatingCollectionsLock);
        }

        public override OperationResult UpdateEntityInfo( bool isSilentUpdate = false )
        {
            if ( !IsEntityInfoValid( Model ) )
            {
                logger.Error( $"The entity basic info is not available. Entity DB ID = {Model?.ID}" );
                Model.CurrentStatus = FiscalEntityStatus.MissingInformation;
                return OperationResult.Failed;
            }

            if ( !IsFiscalNumberValid( Model.FiscalNumber ) )
            {
                Model.CurrentStatus = FiscalEntityStatus.InvalidFiscalNumber;
                return OperationResult.WrongCredentials;
            }

            if ( Worker != null && !isSilentUpdate )
            {
                Worker?.ReportProgress( -1 , $"A atualizar entidade com o nº fiscal {Model.FiscalNumber}" );
            }

            var updateResult = OperationResult.Default;

            if ( !financesWebScraper.IsLoggedIn )
            {
                updateResult = financesWebScraper.LoginEntity( Model );
            }

            if ( updateResult == OperationResult.Success)
            {
                updateResult = BasicInfoDataExtractor.GetData( Model , financesWebScraper );
            }

            if ( updateResult == OperationResult.Success )
            {
                UnitOfWork.Contacts.RemoveEntities( Model.Contacts );

                updateResult = ContactInfoExtractor.GetData( Model , financesWebScraper );
            }

            if ( updateResult == OperationResult.Success )
            {
                UnitOfWork.Vehiecles.RemoveEntities( Model.Vehiecles );

                updateResult = VehiecleInfoExtractor.GetData( Model , financesWebScraper );
            }

            if ( updateResult == OperationResult.Success )
            {
                UnitOfWork.RealEstates.RemoveEntities( Model.RealEstates );

                updateResult = RealEstateInfoExtractor.GetData( Model , financesWebScraper );
            }

            if ( updateResult == OperationResult.Success )
            {
                UnitOfWork.ImiChargeNotes.RemoveEntities( Model.ImiChargeNotes);

                updateResult = ImiChargeNotesInfoExtractor.GetData( Model , financesWebScraper );
            }

            if ( financesWebScraper.IsLoggedIn )
            {
                financesWebScraper.LogOutEntity();
            }

            if ( updateResult == OperationResult.Success )
            {
                Model.CurrentStatus = FiscalEntityStatus.Updated;
            }
            else if (updateResult == OperationResult.Failed)
            {
                Model.CurrentStatus = FiscalEntityStatus.NotUpdated;
            }

            return updateResult;
        }

        public override bool PersistChanges()
        {
            try
            {
                UnitOfWork.Complete();
                return true;
            }
            catch ( Exception ex)
            {
                WindowService.ShowException( ex );
                return false;                
            }
        }

        //internal static void PrintInvalidEntities( string targetFileFullName , BackgroundWorker worker )
        //{
        //    List<FiscalEntityDataModel> invalidEntities = new List<FiscalEntityDataModel>();

        //    worker.ReportProgress( 0 , "A obter entidades invalidas" );

        //    //Get entities in invalid state
        //    using ( IUnitOfWork unitOfWork = new UnitOfWork() )
        //    {
        //        invalidEntities.AddRange( unitOfWork.Clients.GetClientsInInvalidState() );

        //        invalidEntities.AddRange( unitOfWork.Companies.GetCompaniesInInvalidState() );
        //    }

        //    worker.ReportProgress( 50 , "A imprimir entidades invalidas" );

        //    //Print the invalid entities
        //    PdfConversionHandler.PrintObjectToPdf( new { SearchResults = invalidEntities }, InvalidEntitiesTemplatePath , targetFileFullName );
        //}

        #endregion

    }
}