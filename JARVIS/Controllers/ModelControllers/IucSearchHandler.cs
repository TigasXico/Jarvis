using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;

using Jarvis.Data.DataModels;
using Jarvis.DataAccess.Repositories;
using Jarvis.DataAcess.Contract;
using Jarvis.Services;

namespace Jarvis.DataHandlers.Handlers
{
    public class IucSearchHandler
    {
        private const string IUCTemplatePathConfigKey = "IUCTemplateFilePath";
        private static string IUCTemplatePath => ConfigurationManager.AppSettings.Get( IUCTemplatePathConfigKey );

        public int MonthNumberOfSearch
        {
            get;
            set;
        }

        public string MonthNameOfSearch
        {
            get;
            set;
        }

        public IEnumerable<FiscalEntityDataModel> SearchResults
        {
            get;
            private set;
        }

        internal bool ProcessSearch(string targetFileFullName , BackgroundWorker worker )
        {
            using ( IUnitOfWork unitOfWork = new UnitOfWork() )
            {
                worker?.ReportProgress( 0 , "A obter IUC's para o mês selecionado" );

                //make search
                IEnumerable<IGrouping<FiscalEntityDataModel , VehiecleDataModel>> queryResults = unitOfWork.Vehiecles.GetVehieclesWithPlateOnMonthForIUC( MonthNumberOfSearch );

                if ( queryResults.Count() > 0 )
                {
                    SearchResults = queryResults.Select( pair => pair.Key );


                    worker?.ReportProgress( 50 , "A exportar IUC's para o mês selecionado" );

                    //print results to PDF
                    //return PdfConversionHandler.PrintObjectToPdf( this , IUCTemplatePath , targetFileFullName );
                    return true;
                }
                else
                {
                    WindowService.DisplayMessage( MessageType.Information , $"Nenhum veículo encontrado para o mês de {MonthNameOfSearch}." );
                    return true;
                }
            }
        }
    }
}
