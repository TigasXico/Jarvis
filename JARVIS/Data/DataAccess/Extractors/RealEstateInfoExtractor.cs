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
                var success = financesWebScraper.GetPageContent( RealEstateInfoLink , out var infoAsHtml );
                
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
