using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Text;

using HtmlAgilityPack;

using Jarvis.Data.DataModels;
using Jarvis.Interfaces;

using log4net;

using ScrapySharp.Html.Forms;
using ScrapySharp.Network;

namespace Jarvis.DataAccess.WebScraping
{
    public class FinancesWebScraper : FiscalEntityWebScraper
    {
        private readonly ILog logger = LogManager.GetLogger( typeof( FinancesWebScraper ) );

        #region Config Fields

        #region Finances related config fields

        private const string financesHomePageLinkConfigKey = "FinancesHomepageLink";
        private static Uri FinancesHomePageLink => new Uri( ConfigurationManager.AppSettings.Get( financesHomePageLinkConfigKey ) );

        private const string logOutLinkConfigKey = "LogOutLink";
        private static Uri LogOutLink => new Uri( ConfigurationManager.AppSettings.Get( logOutLinkConfigKey ) );

        #endregion

        #region Authentication related config fields

        private const string centralAuthenticationSubmissionLinkConfigKey = "CentralAuthenticationSubmissionLink";
        private static Uri CentralAuthenticationSubmissionLink => new Uri( ConfigurationManager.AppSettings.Get( centralAuthenticationSubmissionLinkConfigKey ) );

        private const string centralLoginFormNameConfigKey = "CentralLoginFormName";
        private static string CentralLoginFormName => ConfigurationManager.AppSettings.Get( centralLoginFormNameConfigKey );

        private const string centralFormUsernameFieldNameConfigKey = "CentralFormUsernameField";
        private static string CentralFormUsernameFieldName => ConfigurationManager.AppSettings.Get( centralFormUsernameFieldNameConfigKey );

        private const string centralFormPasswordFieldNameConfigKey = "CentralFormPasswordField";
        private static string CentralFormPasswordFieldName => ConfigurationManager.AppSettings.Get( centralFormPasswordFieldNameConfigKey );

        private const string centralForwardSubmissFormNameConfigKey = "CentralForwardSubmissFormName";
        private static string CentralForwardSubmissFormName => ConfigurationManager.AppSettings.Get( centralForwardSubmissFormNameConfigKey );

        #endregion

        #endregion

        #region Properties

        public override bool IsLoggedIn
        {
            get;
            protected set;
        }

        #endregion

        #region Initialization methods

        /// <summary>
        /// Assures that the internal webScraper is initialized and ready to use
        /// </summary>
        /// <returns>True if the scraper is initialized sucesfully</returns>
        public override ScrapingBrowser InitScrapper()
        {
            ScrapingBrowser generatedBrowser = new ScrapingBrowser
            {
                AllowAutoRedirect = true ,
                AllowMetaRedirect = true ,
                ClearHeadersAfterRequest = false ,
                ProtocolVersion = new Version( 1 , 1 ) ,
                IgnoreCookies = false ,
                UseDefaultCookiesParser = true ,
                Encoding = Encoding.GetEncoding( "ISO-8859-1" ) ,
                KeepAlive = true
            };

            generatedBrowser.Headers.Add( "Accept-Language" , "pt-PT,pt;q=0.9,en-PT;q=0.8,en;q=0.7,en-US;q=0.6" );
            generatedBrowser.Headers.Add( "Accept" , "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9" );

            return generatedBrowser;
        }

        #endregion

        #region Information Extraction
        
        public override bool GetPage( Uri uri , out WebPage targetPage )
        {
            return GetPageAndContent( uri , out targetPage, out _);
        }

        public override bool GetPageContent( Uri uri , out HtmlNode targetPageHtml )
        {
            return GetPageAndContent( uri , out _ , out targetPageHtml );
        }

        public override bool GetPageAndContent( Uri uri , out WebPage targetPage , out HtmlNode targetPageContent )
        {
            targetPage = null;
            targetPageContent = null;

            if ( !IsLoggedIn)
            {
                logger.Error( "Scraper not loged in yet!" );
                return false;
            }

            //Get map between host and authentication cookie
            NameValueCollection authenticationCookiesMapping = ( NameValueCollection ) ConfigurationManager.GetSection( "AuthenticationCookiesMapping" );

            //Get cookie before navigating
            //Cookie that authenticates on site
            Cookie existingCookie = ScrapingBrowser.GetCookie( uri , authenticationCookiesMapping[uri?.Host] );

            //Navigate to page
            WebPage resultingPage = ScrapingBrowser.NavigateToPage( uri );

            //If cookie did not exist, then entity is not logged in in this host
            //If cookie existed, entity is logged in, no need to forward form
            if ( existingCookie == null )
            {
                //get the form to forward
                PageWebForm forwardSubmissionForm = resultingPage.FindFormById( CentralForwardSubmissFormName );

                //submit the form, the resulting page is the target page
                resultingPage = forwardSubmissionForm.Submit( uri );
            }

            //Store page as result
            targetPage = resultingPage;

            //Get HTML from target web page
            targetPageContent = resultingPage?.Html;

            return resultingPage != null;
        }

        #endregion

        #region Authentication Handling

        public override OperationResult LoginEntity( FiscalEntityDataModel entityToLogIn )
        {
            WebPage loginPage = ScrapingBrowser.NavigateToPage( FinancesHomePageLink );

            if ( loginPage != null)
            {
                PageWebForm loginForm = loginPage.FindFormById( CentralLoginFormName );

                if ( loginForm == null )
                {
                    logger.Error( "Could not find loggin form..." );

                    return OperationResult.Failed;
                }

                FillInCentralLoginForm( ref loginForm , entityToLogIn );

                //Submit the loggin from, to have authentication cookies
                WebPage resultingPage = loginForm.Submit( CentralAuthenticationSubmissionLink );

                PageWebForm forwardSubmissionForm = resultingPage?.FindFormById( CentralForwardSubmissFormName );

                IsLoggedIn = (resultingPage != null && forwardSubmissionForm != null);

                if ( !IsLoggedIn )
                {
                    entityToLogIn.CurrentStatus = FiscalEntityStatus.WrongCredentials;
                }
            }

            return (IsLoggedIn) ? OperationResult.Success : OperationResult.WrongCredentials;
        }

        private void FillInCentralLoginForm( ref PageWebForm loginForm , IFiscalEntity Client )
        {
            //Fix the Path field
            loginForm["path"] = "/geral/dashboard";
            //Fill the username field
            loginForm[CentralFormUsernameFieldName] = Client.FiscalNumber;
            //Fill the password field
            loginForm[CentralFormPasswordFieldName] = Client.FiscalPassword;
        }

        public override void LogOutEntity()
        {
            ScrapingBrowser.NavigateToPage( LogOutLink );
            ScrapingBrowser.ClearCookies();
            IsLoggedIn = false;
        }

        #endregion
    }
}
