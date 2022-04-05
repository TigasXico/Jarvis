using HtmlAgilityPack;
using Jarvis.Data.DataModels;
using Jarvis.Services;

using log4net;

using System;
using System.Configuration;
using Jarvis.Controllers.Contract;
using Jarvis.Data.Contract;
using Jarvis.Data.DataAccess.Parsing;

namespace Jarvis.Data.DataAccess.Extractors
{
    public static class VehiecleInfoExtractor
    {
        private static readonly ILog logger = LogManager.GetLogger( typeof( VehiecleInfoExtractor ) );

        #region Vehiecle info related config fields

        private const string landVehieclesInfoLinkConfigKey = "LandVehieclesInfoLink";
        private static Uri LandVehieclesInfoLink => new Uri( ConfigurationManager.AppSettings.Get( landVehieclesInfoLinkConfigKey ) );

        #endregion

        public static OperationResult GetData( FiscalEntityDataModel entity , IWebScraper<FiscalEntityDataModel> financesWebScraper )
        {
            try
            {
                var success = financesWebScraper.GetPageContent( LandVehieclesInfoLink , out var infoAsHtml );
                
                logger.Info( $"The entity land vehiecles information scrapping was {(success ? string.Empty : "not ") } sucesfull" );

                if ( !success )
                {
                    return OperationResult.Failed;
                }

                success = false;

                success = VehiecleInfoParser.ParseEntityVehieclesInfo( infoAsHtml , entity );

                logger.Info( $"The entity land vehiecles information parsing was {(success ? string.Empty : "not ") } sucesfull" );

                return OperationResult.Success;
            }
            catch ( Exception ex )
            {
                WindowService.ShowException( ex );
                logger.Fatal( $"The entity land vehiecles processing has failed. Details:{Environment.NewLine}{entity}" , ex );
                return OperationResult.Failed;
            }
        }
    }
}
