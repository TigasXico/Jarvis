using HtmlAgilityPack;

using Jarvis.DataAccess.Parsers;
using Jarvis.Data.DataModels;
using Jarvis.Interfaces;
using Jarvis.Services;

using log4net;

using System;
using System.Configuration;

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
                bool success = financesWebScraper.GetPageContent( LandVehieclesInfoLink , out HtmlNode infoAsHtml );
                
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
