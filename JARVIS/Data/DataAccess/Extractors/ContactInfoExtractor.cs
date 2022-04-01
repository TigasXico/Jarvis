using System;
using System.Configuration;

using HtmlAgilityPack;

using Jarvis.Data.DataModels;
using Jarvis.DataAccess.Parsers;
using Jarvis.Interfaces;
using Jarvis.Services;

using log4net;

namespace Jarvis.Data.DataAccess.Extractors
{
    public static class ContactInfoExtractor
    {
        private static readonly ILog logger = LogManager.GetLogger( typeof( ContactInfoExtractor ) );

        #region Contact information related config fields

        private const string contactInfoLinkConfigKey = "ContactInfoLink";
        private static Uri ContactInfoLink => new Uri( ConfigurationManager.AppSettings.Get( contactInfoLinkConfigKey ) );

        #endregion

        public static OperationResult GetData( FiscalEntityDataModel entity , IWebScraper<FiscalEntityDataModel> financesWebScraper )
        {
            try
            {
                bool success = financesWebScraper.GetPageContent( ContactInfoLink , out HtmlNode infoAsHtml );

                logger.Info( $"The entity contact information scrapping was {(success ? string.Empty : "not ") } sucesfull" );

                if ( !success )
                {
                    return OperationResult.Failed;
                }

                success = ContactInfoParser.ParseEntityContactInfo( infoAsHtml , entity );

                logger.Info( $"The entity contact information parsing was {(success ? string.Empty : "not ") } sucesfull" );

                return OperationResult.Success;
            }
            catch ( Exception ex )
            {
                WindowService.ShowException( ex );
                logger.Fatal( $"The entity contact information processing has failed. Details:{Environment.NewLine}{entity}" , ex );
                return OperationResult.Failed;
            }
        }
    }
}
