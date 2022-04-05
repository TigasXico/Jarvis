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
    public static class BasicInfoDataExtractor
    {
        private static readonly ILog logger = LogManager.GetLogger( typeof( BasicInfoDataExtractor ) );

        #region General info related config fields

        private static string TargetLinkConfigKey => "ClientGeneralInfoLink";

        private static Uri TargetLink => new Uri( ConfigurationManager.AppSettings.Get( TargetLinkConfigKey ) );

        #endregion

        public static OperationResult GetData( FiscalEntityDataModel entity , IWebScraper<FiscalEntityDataModel> financesWebScraper )
        {
            try
            {
                var success = financesWebScraper.GetPageContent( TargetLink , out var infoAsHtml );

                logger.Info( $"The entity basic information scrapping was {(success ? string.Empty : "not ") } sucesfull" );

                if ( !success )
                {
                    return OperationResult.Failed;
                }

                success = false;

                if ( entity is ClientDataModel client )
                {
                    success = ClientBasicInfoParser.ParseClientBasicInfo( infoAsHtml , ref client );
                }
                else if ( entity is CompanyDataModel company )
                {
                    success = CompanyBasicInfoParser.ParseCompanyBasicInfo( infoAsHtml , ref company );
                }

                logger.Info( $"The entity basic information parsing was {(success ? string.Empty : "not ") }sucesfull" );

                return OperationResult.Success;
            }
            catch ( Exception ex )
            {
                WindowService.ShowException( ex );
                logger.Fatal( $"The entity basic information processing has failed. Details:{Environment.NewLine}{entity}" , ex );
                return OperationResult.Failed;
            }
        }

    }
}
