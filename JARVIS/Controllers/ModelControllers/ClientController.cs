using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;

using Jarvis.Controllers.ScreenControllers;
using Jarvis.Data.Contract.Repositories;
using Jarvis.Data.DataModels;
using Jarvis.Services;

namespace Jarvis.Controllers.ModelControllers
{
    public class ClientController : FiscalEntityController
    {
        #region Configurations related to client

        private const string AggregateSheetTemplatePathConfigKey = "AggregateSheetTemplateFilePath";
        private static string AggregateSheetTemplatePath => ConfigurationManager.AppSettings.Get( AggregateSheetTemplatePathConfigKey );

        #endregion

        #region Constructor

        public ClientController( ClientDataModel client , BackgroundWorker worker , IUnitOfWork unitOfWork = null ) : base( worker , unitOfWork )
        {
            var obtainedClientFromDatabase = UnitOfWork.Clients.GetByFiscalNumber( client.FiscalNumber );

            if ( obtainedClientFromDatabase != default( ClientDataModel ) )
            {
                Model = obtainedClientFromDatabase;
            }
            else
            {
                UnitOfWork.Clients.Add( client );
                Model = client;
                Model.IsNew = true;
            }

            PrepareCollectionsForUpdate();
        }

        internal static void UpdateAggregateSelection( IEnumerable<AggregateDataModel> enumerable , out AggregateDataModel selectedAggregate )
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Methods

        //public static void PrintAgregateOfClientToPdf( ClientDataModel client , string targetFileFullName , BackgroundWorker worker )
        //{
        //    worker?.ReportProgress( -1 , $"A imprimir ficha de agregado do cliente {client.Name} - {client.FiscalNumber}" );

        //    PdfConversionHandler.PrintObjectToPdf( client, AggregateSheetTemplatePath , targetFileFullName );
        //}

        public static bool GetAggregateSelection( IEnumerable<AggregateDataModel> existingAggregates , out AggregateDataModel selectedAggregate )
        {
            selectedAggregate = null;

            //Open window to update aggregate
            var screenController =
                new SelectAggregateScreenController( existingAggregates )
                {
                    DisplayControlButtons = true ,
                    EnableControls = true ,
                    ReadOnlyControlls = false ,
                    SearchLabel = "Agregado :"
                };

            if ( WindowService.ShowWindowForController( screenController , "Atualizar agregado" ) )
            {
                selectedAggregate = ExtractResult( screenController );
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool DeleteEntity()
        {
            try
            {
                UnitOfWork.Clients.RemoveEntity( Model as ClientDataModel);
                return true;
            }
            catch ( Exception ex )
            {
                WindowService.ShowException( ex );
                return false;
            }
        }

        #endregion
    }
}
