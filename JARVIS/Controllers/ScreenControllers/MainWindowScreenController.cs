using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

using GalaSoft.MvvmLight.CommandWpf;
using Jarvis.Controllers.Contract;
using Jarvis.Controllers.ModelControllers;
using Jarvis.Controllers.ModelControllers.Factories;
using Jarvis.Controllers.ScreenControllers.Factories;
using Jarvis.Data.Contract;
using Jarvis.Data.Contract.Repositories;
using Jarvis.Data.DataAccess.Repositories;
using Jarvis.Data.DataModels;
using Jarvis.Services;
using Jarvis.Utils.HelperClasses;

using log4net;

namespace Jarvis.Controllers.ScreenControllers
{
    public class MainWindowScreenController : PropertyChangedRaiser
    {
        #region Variables

        private readonly ILog logger = LogManager.GetLogger( typeof( MainWindowScreenController ) );

        #endregion

        #region Properties

        #region Entities management Properties

        public IDataModelScreenController<FiscalEntityDataModel> CurrentEntityScreenController => CurrentTabScreenController?.WrappedObject;

        public IDataModel CurrentEntity => CurrentEntityController?.Model;

        private IUpdatableDataModelController<FiscalEntityDataModel> currentEntityController;
        public IUpdatableDataModelController<FiscalEntityDataModel> CurrentEntityController
        {
            get => currentEntityController;
            set => SetProperty( ref currentEntityController , value );
        }

        private TabScreenController currentTabScreenController;
        public TabScreenController CurrentTabScreenController
        {
            get => currentTabScreenController;
            set
            {
                SetProperty( ref currentTabScreenController , value );
                CurrentEntityController = CurrentEntityScreenController?.DataModelController;
            }
        }

        private ObservableCollection<TabScreenController> entitiesBeingEdited;
        public ObservableCollection<TabScreenController> EntitiesBeingEdited
        {
            get => entitiesBeingEdited;
            set => SetProperty( ref entitiesBeingEdited , value );
        }

        #endregion

        #region Commands

        #region Entities management

        public ICommand AddEntityCommand
        {
            get;
            private set;
        }

        public ICommand SaveEntityCommand
        {
            get;
            private set;
        }

        public ICommand RemoveEntityCommand
        {
            get;
            private set;
        }

        #endregion

        #region Entities operation

        public ICommand SearchEntityCommand
        {
            get;
            private set;
        }

        public ICommand SearchClientCommand
        {
            get;
            private set;
        }

        public ICommand SearchCompanyCommand
        {
            get;
            private set;
        }

        public ICommand UpdateSelectedEntityCommand
        {
            get;
            private set;
        }

        public ICommand UpdateMultipleEntitiesCommand
        {
            get;
            private set;
        }

        public ICommand UpdateAllEntitiesCommand
        {
            get;
            private set;
        }

        #endregion

        #region Entites export

        public ICommand PrintAggregateCommand
        {
            get;
            private set;
        }

        public ICommand PrintIUCCommand
        {
            get;
            private set;
        }

        public ICommand PrintIMICommand
        {
            get;
            private set;
        }

        public ICommand PrintInvalidEntitiesCommand
        {
            get;
            private set;
        }

        #endregion

        #region Entities import

        public ICommand ImportFromCSV
        {
            get;
            private set;
        }

        #endregion

        #endregion

        #region Other properties

        private bool loading;
        public bool Loading
        {
            get => loading;
            set => SetProperty( ref loading , value );
        }

        private string actionText;
        public string ActionText
        {
            get => actionText;
            set => SetProperty( ref actionText , value );
        }

        #endregion

        #endregion

        #region Constructors

        public MainWindowScreenController()
        {
            //Load the entities that will be needed from the get go
            using ( IUnitOfWork tempUnit = new UnitOfWork() )
            {
                tempUnit.Clients.Get();
                tempUnit.Companies.Get();
            }

            AddEntityCommand = new RelayCommand( AddNewEntityAction );
            SaveEntityCommand = new RelayCommand( SaveEntityAction , IsEntitySelected );
            RemoveEntityCommand = new RelayCommand( RemoveEntityAction , IsEntitySelected );

            SearchEntityCommand = new RelayCommand( SearchEntityAction );
            SearchClientCommand = new RelayCommand( SearchClientAction );
            SearchCompanyCommand = new RelayCommand( SearchCompanyAction );

            UpdateSelectedEntityCommand = new RelayCommand( UpdateSelectedEntityAction , IsEntitySelected );
            UpdateMultipleEntitiesCommand = new RelayCommand( UpdateMultipleEntitiesAction );
            UpdateAllEntitiesCommand = new RelayCommand( UpdateAllEntitiesAction );

            //PrintAggregateCommand = new RelayCommand( PrintAggregatePdfAction , IsEntitySelected );
            PrintIUCCommand = new RelayCommand( PrintIUCAction );
            PrintIMICommand = new RelayCommand( PrintIMIAction );
            //PrintInvalidEntitiesCommand = new RelayCommand( PrintInvalidEntitiesAction );

            ImportFromCSV = new RelayCommand( ImportFromCSVAction );

            EntitiesBeingEdited = new ObservableCollection<TabScreenController>();


        }

        #endregion

        #region Methods

        #region Initialization

        private void InitWorker( BackgroundWorker worker )
        {
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;

            worker.ProgressChanged += ( object sender , ProgressChangedEventArgs e ) =>
            {
                ActionText = e.UserState.ToString();

                if ( e.ProgressPercentage != -1 )
                {
                    ActionText += $" - Estado: {e.ProgressPercentage}%";
                }
            };
        }

        private void ResetLoading( BackgroundWorker worker = null )
        {
            Loading = false;

            ActionText = string.Empty;

            worker?.Dispose();
        }

        #endregion

        #region Search

        private void ShowSearchScreen( SearchMode searchMode )
        {
            var searchScreenController = new SearchScreenController
            {
                SearchMode = searchMode
            };

            string title = null;

            switch ( searchMode )
            {
                case SearchMode.SearchAll:
                    title = "Pesquisar Tudo";
                    break;
                case SearchMode.SearchFiscalEntity:
                    title = "Pesquisar Entidades Fiscais";
                    break;
                case SearchMode.SearchClient:
                    title = "Pesquisar Clientes";
                    break;
                case SearchMode.SearchCompany:
                    title = "Pesquisar Empresas";
                    break;
                case SearchMode.SearchVehiecle:
                    title = "Pesquisar Veículos";
                    break;
                case SearchMode.SearchRealEstate:
                    title = "Pesquisar Imóveis";
                    break;
                default:
                    break;
            }

            if ( WindowService.ShowWindowForController( searchScreenController , title ) )
            {
                var selectedEntries = searchScreenController.SelectedEntries;

                LoadEntitiesListForEditing( selectedEntries );
            }
        }

        private void LoadEntitiesListForEditing( List<IDataModel> entitiesForEditing)
        {
            if ( entitiesForEditing.Count > 20 )
            {
                WindowService.DisplayMessage( MessageType.Warning , $"Demasiados resultados selecionados.{Environment.NewLine}Apenas os primeiros 20 resultados serão mostrados" , "Demasiados resultados selecionados" );
                entitiesForEditing.RemoveRange( 20 , entitiesForEditing.Count - 20 );
            }

            EntitiesBeingEdited.Clear();

            foreach ( FiscalEntityDataModel selectedEntry in entitiesForEditing )
            {
                LoadEntityForEditing( selectedEntry );
            }
        }

        #endregion

        #region Data handling

        private void LoadEntityForEditing( IFiscalEntity entity , bool passWorkerToController = true )
        {
            if ( entity != null )
            {
                IUpdatableDataModelController<FiscalEntityDataModel> entityController;

                if ( passWorkerToController )
                {
                    var worker = new BackgroundWorker();

                    InitWorker( worker );

                    entityController = DataModelControllerFactory.GetControllerForEntity( entity , worker );
                }
                else
                {
                    entityController = DataModelControllerFactory.GetControllerForEntity( entity );
                }

                if ( entityController != null )
                {
                    CurrentEntityController = entityController;

                    var screenController = DataModelScreenControllerFactory.GetScreenControllerForEntity( entityController );

                    if ( screenController != null )
                    {
                        CurrentTabScreenController = new TabScreenController( screenController , EntitiesBeingEdited )
                        {
                            DisplaySaveButtons = true ,
                        };

                        EntitiesBeingEdited.Add( CurrentTabScreenController );
                    }

                    entityController.Model.IsDirty = false;
                }
            }
            else
            {
                WindowService.DisplayMessage( MessageType.Error , "Controller not set for loading" , "Error" );
            }
        }

        private void ProcessResults( List<ProcessingResult> processingResults , bool loadProcessedEntities )
        {
            EntitiesBeingEdited.Clear();

            if ( loadProcessedEntities )
            {
                LoadEntitiesListForEditing(processingResults.Select(e => e.Entity as IDataModel).ToList());
            }

            DisplayProcessingResult( processingResults );
        }

        #endregion

        #region Screen showing

        private bool GetPathToSaveFile( string fileName , out string fullFileName )
        {
            fullFileName = null;

            var saveSheetDialog = new SaveFileDialog()
            {
                CheckPathExists = true ,
                Title = "Local para guardar PDF" ,
                DefaultExt = "pdf" ,
                Filter = "PDF File (*.pdf) | *.pdf" ,
                AddExtension = true ,
                FileName = fileName
            };

            var dialogResult = saveSheetDialog.ShowDialog();



            if ( dialogResult == DialogResult.OK )
            {
                fullFileName = saveSheetDialog.FileName;
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #endregion

        #region Commands

        #region Entities management

        private void AddNewEntityAction()
        {
            IUnitOfWork unitOfWork = new UnitOfWork();

            var newEntityScreenController = new NewFiscalEntityScreenController( unitOfWork )
            {
                DisplayControlButtons = true
            };

            if ( WindowService.ShowWindowForController( newEntityScreenController , "Adicionar nova entidade" ) )
            {
                var worker = new BackgroundWorker();

                InitWorker( worker );

                worker.DoWork += ( object sender , DoWorkEventArgs e ) =>
                {
                    var processingResults = new List<ProcessingResult>();

                    Loading = true;

                    var counter = 0;
                    var totalToProcess = newEntityScreenController.CredentialsBeingEdited.Count;

                    foreach ( var credentials in newEntityScreenController.CredentialsBeingEdited )
                    {
                        var percentage = ModuleUtils.CalculatePercentage( ++counter , totalToProcess );

                        //report progress here, not on the controller , for total loading percentage
                        worker?.ReportProgress( percentage , $"A processar entidade {counter}/{totalToProcess}" );

                        FiscalEntityController.ResolveFiscalInfoToEntity( credentials.FiscalNumber , credentials.FiscalPassword , out var generatedEntity );

                        var controller = DataModelControllerFactory.GetControllerForEntity( generatedEntity , worker , unitOfWork );

                        var updateResult = controller.UpdateEntityInfo(true);

                        if ( newEntityScreenController.SelectedAggregate != null && generatedEntity is ClientDataModel client )
                        {

                            client.Aggregate = newEntityScreenController.SelectedAggregate;

                        }

                        processingResults.Add( new ProcessingResult( generatedEntity , updateResult ) );

                    }

                    unitOfWork.Complete();

                    unitOfWork.Dispose();

                    e.Result = processingResults;
                };

                worker.RunWorkerCompleted += ( object sender , RunWorkerCompletedEventArgs e ) =>
                {
                    if ( e.Result is List<ProcessingResult> processingResults )
                    {
                        ProcessResults( processingResults , true );
                    }

                    ResetLoading( worker );
                };

                worker.RunWorkerAsync();
            }
        }

        private void SaveEntityAction()
        {
            var successfullSaved = CurrentEntityController.PersistChanges();

            if ( successfullSaved )
            {
                CurrentTabScreenController.RemoveFromScreen();
            }
        }

        private void RemoveEntityAction()
        {
            var confirmation = WindowService.DisplayMessage( MessageType.Confirmation , $"Tem a certeza que pretende apagar a entidade {CurrentEntity?.CommonId} ?" );

            if ( confirmation.HasValue && confirmation.Value )
            {
                var successfullRemoval = CurrentEntityController.DeleteEntity();

                if ( successfullRemoval )
                {
                    SaveEntityAction();

                    WindowService.DisplayMessage( MessageType.Information , "Entitade removida com sucesso!" );
                }
            }
        }

        #endregion

        #region Entities operation

        private void SearchEntityAction()
        {
            ShowSearchScreen( SearchMode.SearchFiscalEntity );
        }

        private void SearchClientAction()
        {
            ShowSearchScreen( SearchMode.SearchClient );
        }

        private void SearchCompanyAction()
        {
            ShowSearchScreen( SearchMode.SearchCompany );
        }

        private void UpdateSelectedEntityAction()
        {
            var result = WindowService.DisplayMessage(
                MessageType.Confirmation ,
                $"Começar processo de atualização para {CurrentEntity?.CommonId}?" ,
                "Confirmar atualização de informação" );

            if ( result.HasValue && result.Value )
            {
                var worker = new BackgroundWorker();

                InitWorker( worker );

                worker.DoWork += ( object sender , DoWorkEventArgs e ) =>
                {
                    var processingResult = CurrentEntityController.UpdateEntityInfo();

                    e.Result = new ProcessingResult( CurrentEntityController.Model , processingResult );
                };

                worker.RunWorkerCompleted += ( object sender , RunWorkerCompletedEventArgs e ) =>
                {
                    if ( e.Result is List<ProcessingResult> processingResults )
                    {
                        ProcessResults( processingResults , true );
                    }

                    ResetLoading( worker );
                };

                Loading = true;

                worker.RunWorkerAsync();
            }
        }

        private void UpdateMultipleEntitiesAction()
        {
            var searchScreenController = new SearchScreenController
            {
                SearchMode = SearchMode.SearchFiscalEntity
            };

            if ( WindowService.ShowWindowForController( searchScreenController , "Atualizar Multiplas Entidades" ) )
            {
                var worker = new BackgroundWorker();

                InitWorker( worker );

                worker.DoWork += ( object sender , DoWorkEventArgs e ) =>
                {
                    var processingResults = new List<ProcessingResult>();

                    var counter = 0;

                    var totalToProcess = searchScreenController.SelectedEntries.Count;

                    foreach ( FiscalEntityDataModel selectedEntry in searchScreenController.SelectedEntries )
                    {
                        var percentage = ModuleUtils.CalculatePercentage( ++counter , totalToProcess );

                        //report progress here, not on the controller , for total loading percentage
                        worker?.ReportProgress( percentage , $"A processar entidade {counter}/{totalToProcess} - " );

                        var controller = DataModelControllerFactory.GetControllerForEntity( selectedEntry , worker );

                        var resultOfCurrentProcessing = controller.UpdateEntityInfo();

                        processingResults.Add( new ProcessingResult( selectedEntry , resultOfCurrentProcessing ) );
                    }

                    e.Result = processingResults;

                };

                worker.RunWorkerCompleted += ( object sender , RunWorkerCompletedEventArgs e ) =>
                {
                    if ( e.Result is List<ProcessingResult> processingResults )
                    {
                        ProcessResults( processingResults, false );
                    }

                    ResetLoading( worker );
                };

                Loading = true;

                worker.RunWorkerAsync();
            }
        }

        private void UpdateAllEntitiesAction()
        {
            Loading = true;

            var allEntities = new List<FiscalEntityDataModel>();
            
            using ( IUnitOfWork unitOfWork = new UnitOfWork() )
            {
                allEntities.AddRange( unitOfWork.Clients.GetAll() );

                allEntities.AddRange( unitOfWork.Companies.GetAll() );
            }

            var message = $"Atualizar todas as {allEntities.Count} entidades poderá demorar algum tempo. De certeza que pretende prosseguir?";

            var confirmationResult = WindowService.DisplayMessage( MessageType.Confirmation , message , "Atualizar TODAS as Entidades" );

            if ( confirmationResult.HasValue && confirmationResult.Value )
            {
                var worker = new BackgroundWorker();

                InitWorker( worker );

                worker.DoWork += ( object sender , DoWorkEventArgs e ) =>
                {
                    var processingResults = new List<ProcessingResult>();

                    var counter = 0;
                    var totalToProcess = allEntities.Count;

                    using ( IUnitOfWork unitOfWork = new UnitOfWork() )
                    {
                        foreach ( var selectedEntry in allEntities )
                        {
                            var percentage = ModuleUtils.CalculatePercentage( ++counter , totalToProcess );

                            //report progress here, not on the controller , for total loading percentage
                            worker?.ReportProgress( percentage , $"A processar entidade {counter}/{totalToProcess} - " );

                            var controller = DataModelControllerFactory.GetControllerForEntity( selectedEntry , worker , unitOfWork );

                            var resultOfCurrentProcessing = controller.UpdateEntityInfo( true );

                            processingResults.Add( new ProcessingResult( selectedEntry , resultOfCurrentProcessing ) );
                        }

                        unitOfWork.Complete();
                    }

                    e.Result = processingResults;
                };

                worker.RunWorkerCompleted += ( object sender , RunWorkerCompletedEventArgs e ) =>
                {
                    if ( e.Result is List<ProcessingResult> processingResults )
                    {
                        ProcessResults( processingResults , false );
                    }

                    ResetLoading( worker );
                };

                worker.RunWorkerAsync();
            }
            else
            {
                ResetLoading();
            }
        }

        private static void DisplayProcessingResult( List<ProcessingResult> processingResults )
        {
            var processingResultScreenController = new EntitiesProcessingResultScreenController()
            {
                EntitiesProcessingStatus = processingResults
            };

            WindowService.ShowWindowForController( processingResultScreenController , "Resultado da importação" );
        }

        #endregion

        #region Entites export

        //private void PrintAggregatePdfAction()
        //{
        //    string targetFileName = $"Ficha informativa de {CurrentEntity?.CommonId}";

        //    if ( GetPathToSaveFile( targetFileName , out string targetFileFullName ) )
        //    {
        //        BackgroundWorker worker = new BackgroundWorker();

        //        InitWorker( worker );

        //        worker.DoWork += ( object sender , DoWorkEventArgs e ) =>
        //        {
        //            switch ( CurrentEntityScreenController.Entity )
        //            {
        //                case ClientDataModel clientModel:
        //                    ClientController.PrintAgregateOfClientToPdf( clientModel , targetFileFullName , worker );
        //                    break;
        //            }

        //            e.Result = new ProcessingResult( CurrentEntityScreenController.Entity , OperationResult.Success );

        //        };

        //        worker.RunWorkerCompleted += ( object sender , RunWorkerCompletedEventArgs e ) =>
        //        {
        //            if ( e.Result is ProcessingResult processingResult )
        //            {
        //                ProcessResults( new List<ProcessingResult>() { processingResult } , false );
        //            }

        //            ResetLoading( worker );
        //        };

        //        Loading = true;

        //        worker.RunWorkerAsync();
        //    }
        //}

        private void PrintIUCAction()
        {
            var selectIucMonthScreenHandler = new IucMonthSelectionScreenController();

            if ( WindowService.ShowWindowForController( selectIucMonthScreenHandler , "Imprimir IUC" ) )
            {
                var printIucHandler = new IucSearchHandler()
                {
                    MonthNameOfSearch = selectIucMonthScreenHandler.SelectedMonthName ,
                    MonthNumberOfSearch = selectIucMonthScreenHandler.SelectedMonthNumber
                };

                var targetFileName = $"IUC - {printIucHandler.MonthNameOfSearch}";

                if ( GetPathToSaveFile( targetFileName , out var targetFileFullName ) )
                {
                    var worker = new BackgroundWorker();

                    InitWorker( worker );

                    worker.DoWork += ( object sender , DoWorkEventArgs e ) =>
                    {
                        printIucHandler.ProcessSearch( targetFileFullName , worker );

                        e.Result = new ProcessingResult( null , OperationResult.Success );

                    };

                    worker.RunWorkerCompleted += ( object sender , RunWorkerCompletedEventArgs e ) =>
                    {
                        var processingResult = e.Result as ProcessingResult;

                        if ( processingResult.Result == OperationResult.Success )
                        {
                            WindowService.DisplayMessage( MessageType.Information , $"Impressão de IUC para o mês de {selectIucMonthScreenHandler.SelectedMonthName} concluída com sucesso!" );
                        }
                        else
                        {
                            WindowService.DisplayMessage( MessageType.Error , $"Ouve um erro na Impressão de IUC para o mês de {selectIucMonthScreenHandler.SelectedMonthName}" );
                        }

                        ResetLoading( worker );
                    };

                    Loading = true;

                    worker.RunWorkerAsync();
                }
            }
        }

        private void PrintIMIAction()
        {
        }

        //private void PrintInvalidEntitiesAction()
        //{
        //    string targetFileName = $"Entidades invalidas";

        //    if ( GetPathToSaveFile( targetFileName , out string targetFileFullName ) )
        //    {
        //        BackgroundWorker worker = new BackgroundWorker();

        //        InitWorker( worker );

        //        worker.DoWork += ( object sender , DoWorkEventArgs e ) =>
        //        {
        //            FiscalEntityController.PrintInvalidEntities( targetFileFullName , worker );
        //        };

        //        worker.RunWorkerCompleted += ( object sender , RunWorkerCompletedEventArgs e ) =>
        //        {
        //            WindowService.DisplayMessage( MessageType.Information , $"Entidades em estado invalido exportadas com sucesso" );

        //            ResetLoading( worker );
        //        };

        //        Loading = true;

        //        worker.RunWorkerAsync();
        //    }
        //}

        #endregion

        #region Entities import

        private void ImportFromCSVAction()
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = false ,
                Filter = "CSV Files(*.csv ) | *.csv"
            };

            if ( openFileDialog.ShowDialog() == DialogResult.OK )
            {


                var worker = new BackgroundWorker();

                InitWorker( worker );

                worker.DoWork += ( object sender , DoWorkEventArgs e ) =>
                {
                    Loading = true;

                    var processingResults = CSVImportHandler.ProcessImport( openFileDialog.FileName , worker );

                    e.Result = processingResults;
                };

                worker.RunWorkerCompleted += ( object sender , RunWorkerCompletedEventArgs e ) =>
                {
                    if ( e.Result is List<ProcessingResult> processingResults )
                    {
                        ProcessResults( processingResults , false );
                    }

                    ResetLoading( worker );
                };

                worker.RunWorkerAsync();
            }
        }

        #endregion

        #region Predicates

        private bool IsEntitySelected()
        {
            return CurrentTabScreenController != null && CurrentEntityScreenController != null;
        }

        #endregion

        #endregion
    }
}
