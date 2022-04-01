
using HtmlAgilityPack;

using Jarvis.Views;

namespace WebScrappingTest
{
    internal class Program
    {
        private static void Main( string[] args )
        {
            //ScrapingBrowser browser = new ScrapingBrowser
            //{
            //    AllowAutoRedirect = true ,
            //    AllowMetaRedirect = true ,
            //    ProtocolVersion = new Version( 1 , 1 ) ,
            //    IgnoreCookies = false ,
            //    UseDefaultCookiesParser = true ,
            //    KeepAlive = true
            //};

            //WebPage pageResult = browser.NavigateToPage( new Uri( @"https://sitfiscal.portaldasfinancas.gov.pt/geral/dashboard" ) );

            ////First login form
            //PageWebForm loginForm = pageResult.FindFormById( "loginForm" );

            //loginForm[userNameFormId] = "178599131";
            //loginForm[passwordFormId] = "3726CISCO";
            //loginForm["path"] = "/geral/dashboard";
            //WebPage responseForLoggin = loginForm.Submit( new Uri( @"https://www.acesso.gov.pt/v2/login" ) );

            ////Second login form
            //loginForm = responseForLoggin.FindFormById( "forwardParticipantForm" );
            //responseForLoggin = loginForm.Submit( new Uri( @"https://sitfiscal.portaldasfinancas.gov.pt/geral/dashboard" ) );

            ////Third login form
            //WebPage anotherLoginPage = browser.NavigateToPage( new Uri( @"https://www.acesso.gov.pt/loginRedirectForm?partID=PFIN&path=main.jsp%3Fbody%3D%2Fexternal%2Fsgrcsitcad%2Fjsp%2FsitcadDadosGerais.do" ) );

            //PageWebForm anotherLoginForm = anotherLoginPage.FindFormById( "forwardParticipantForm" );

            //WebPage clientInfo = anotherLoginForm.Submit( new Uri( "https://www.portaldasfinancas.gov.pt/main.jsp?body=/external/sgrcsitcad/jsp/sitcadDadosGerais.do" ) );

            //ClientModel tempClient1 = null;
            //ClientInfoParser.ParseClientInfo( clientInfo.Html , ref tempClient1 );

            //ClientModel tempClient2 = new ClientModel()
            //{
            //    FiscalNumber = "505901447" ,
            //    FiscalPassword = "JF505901"
            //};

            //ClientHandler.GetClientBasicInfo( ref tempClient2 );
            //ClientHandler.GetClientLandVehiecles( ref tempClient2 );

            //Console.WriteLine( "First ClientModel:" );
            //Console.WriteLine( tempClient1 );

            //Console.WriteLine( "----------------------------------" );

            //Console.WriteLine( "Second ClientModel:" );
            //Console.WriteLine( tempClient2 );
            //Console.WriteLine( "Second ClientModel Vehiecles:" );

            //foreach ( Vehiecle vehiecle in tempClient2.Vehiecles )
            //{
            //    Console.WriteLine( vehiecle );
            //}

            //Console.ReadLine();


            //Fourth login form
            //anotherLoginPage = browser.NavigateToPage( new Uri( @"https://veiculos.portaldasfinancas.gov.pt/consulta/automoveis/consultar" ) );
            //anotherLoginForm = anotherLoginPage.FindFormById( "forwardParticipantForm" );

            //WebPage clientVehiecleInfo = anotherLoginForm.Submit( new Uri( "https://veiculos.portaldasfinancas.gov.pt/consulta/automoveis/consultar" ) );
            //List<HtmlNode> allListedVehiecles = clientVehiecleInfo.Html.CssSelect( "#tabela" ).CssSelect( "tr" ).ToList();

            //foreach ( HtmlNode currentVehiecle in allListedVehiecles )
            //{
            //    HtmlNode[] allvehiecleInfo = currentVehiecle.CssSelect( "td" ).ToArray();

            //    tempClient1.Vehiecles.Add( new Vehiecle( allvehiecleInfo ) );

            //}

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

        }

        public static string GetFieldValueClean( HtmlNode[] array , int index )
        {
            HtmlNode node = array[index];
            return ClearString( node.InnerText );
        }

        private static string ClearString( string textToClean )
        {
            string cleanedString = textToClean.Replace( "\\r\\n" , string.Empty );
            cleanedString = cleanedString.Replace( "\\t" , string.Empty );
            cleanedString = cleanedString.Trim();
            return cleanedString;
        }
    }
}
