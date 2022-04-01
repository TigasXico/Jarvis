using System;
using System.Configuration;

using HtmlAgilityPack;

using Jarvis.Data.DataAccess.Parsing;
using Jarvis.Data.DataModels;
using Jarvis.Interfaces;
using Jarvis.Services;

using log4net;

using ScrapySharp.Html.Forms;
using ScrapySharp.Network;

namespace Jarvis.Data.DataAccess.Extractors
{
    public static class ImiChargeNotesInfoExtractor
    {
        private static readonly ILog logger = LogManager.GetLogger( typeof( ImiChargeNotesInfoExtractor ) );

        #region IMI payment information related config fields

        private const string ImiChargeNotesInfoLinkConfigKey = "ImiChargeNotesInfoLink";
        private static Uri ImiChargeNotesInfoLink => new Uri( ConfigurationManager.AppSettings.Get( ImiChargeNotesInfoLinkConfigKey ) );

        private const string ImiPaymentInfoFormNameConfigKey = "ImiChargeNotesInfoFormName";
        private static string ImiPaymentInfoFormName => ConfigurationManager.AppSettings.Get( ImiPaymentInfoFormNameConfigKey);

        #endregion

        public static OperationResult GetData( FiscalEntityDataModel entity , IWebScraper<FiscalEntityDataModel> financesWebScraper )
        {
            try
            {
                bool success = financesWebScraper.GetPage( ImiChargeNotesInfoLink , out WebPage page );

                logger.Info( $"The entity IMI payments form scrapping was {(success ? string.Empty : "not ") } sucesfull" );

                if ( !success )
                {
                    return OperationResult.Failed;
                }

                PageWebForm getImiChardeNotesForYearForm = page.FindFormByAttribute( "name" , ImiPaymentInfoFormName );

                //IEnumerable<string> queriableYears = pageContent.CssSelect( ".body-link" ).Select( item => item.InnerText);

                //foreach ( string year in queriableYears )
                //{

                string year = DateTime.Now.Year.ToString();

                getImiChardeNotesForYearForm["ano"] = DateTime.Now.AddYears(-1).Year.ToString();

                getImiChardeNotesForYearForm.Action = "/pt/main.jsp";

                WebPage imiChargeNotesForYearPage = getImiChardeNotesForYearForm.Submit();

                HtmlNode content = imiChargeNotesForYearPage.Html;

                success = ImiChargeNotesInfoParser.GetInfoOfImiPaymentOffYear( content , entity );

                logger.Info( $"The entity IMI chate notes parsing for year {year} was {(success ? string.Empty : "not ") } succesfull" );

                if ( !success )
                {
                    return OperationResult.Failed;
                }
                //}

                return OperationResult.Success;
            }
            catch ( Exception ex )
            {
                WindowService.ShowException( ex );
                logger.Fatal( $"The retrieval of the entity IMI payemnt years processing has failed. Details:{Environment.NewLine}" , ex );
                return OperationResult.Failed;
            }
        }
    }
}
