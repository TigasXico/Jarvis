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
    public static class RealEstateInfoExtractor
    {
        private static readonly ILog logger = LogManager.GetLogger( typeof( BasicInfoDataExtractor ) );

        #region Real Estate related config fields

        private const string realEstateInfoLinkConfigKey = "RealEstateInfoLink";
        private static Uri RealEstateInfoLink => new Uri( ConfigurationManager.AppSettings.Get( realEstateInfoLinkConfigKey ) );

        #endregion

        public static OperationResult GetData( FiscalEntityDataModel entity , IWebScraper<FiscalEntityDataModel> financesWebScraper )
        {
            try
            {
                bool success = financesWebScraper.GetPageContent( RealEstateInfoLink , out HtmlNode infoAsHtml );
                
                logger.Info( $"The entity real estate information scrapping was {(success ? string.Empty : "not ") } sucesfull" );

                if ( !success )
                {
                    return OperationResult.Failed;
                }

                success = RealEstateInfoParser.ParseEntityRealEstatetInfo( infoAsHtml , entity );

                logger.Info( $"The entity real estate information parsing was {(success ? string.Empty : "not ") } sucesfull" );

                return OperationResult.Success;
            }
            catch ( Exception ex )
            {
                WindowService.ShowException( ex );
                logger.Fatal( $"The client real estate information processing has failed. Details:{Environment.NewLine}{entity}" , ex );
                return OperationResult.Failed;
            }


        }
    }
}
