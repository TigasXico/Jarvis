using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using HtmlAgilityPack;

using ScrapySharp.Cache;
using ScrapySharp.Extensions;
using ScrapySharp.Html;
using ScrapySharp.Html.Forms;

namespace ScrapySharp.Network
{
    public class WebPage
    {
        private readonly ScrapingBrowser browser;
        private readonly Uri absoluteUrl;
        private readonly RawRequest rawRequest;
        private readonly RawResponse rawResponse;
        private readonly bool autoDetectCharsetEncoding;
        private string content;
        private readonly List<WebResource> resources;
        private HtmlNode html;
        private string baseUrl;

        private static readonly Dictionary<string , string> resourceTags = new Dictionary<string , string>
            {
                {"img", "src"},
                {"script", "src"},
                {"link", "href"},
            };
        public Encoding Encoding
        {
            get; private set;
        }

        public WebPage( ScrapingBrowser browser , Uri absoluteUrl , bool autoDownloadPagesResources , RawRequest rawRequest , RawResponse rawResponse ,
            Encoding encoding , bool autoDetectCharsetEncoding )
        {
            this.browser = browser;
            this.absoluteUrl = absoluteUrl;
            this.rawRequest = rawRequest;
            this.rawResponse = rawResponse;
            this.autoDetectCharsetEncoding = autoDetectCharsetEncoding;
            Encoding = encoding;

            content = Encoding.GetString( rawResponse.Body );
            resources = new List<WebResource>();

            LoadHtml();

            if ( autoDownloadPagesResources )
            {
                LoadBaseUrl();
                DownloadResources();
            }
        }

        private void LoadHtml()
        {
            try
            {
                html = content.ToHtmlNode();
                if ( autoDetectCharsetEncoding )
                {
                    string charset = html.Descendants( "meta" ).Select( meta => meta.GetAttributeValue( "charset" , string.Empty ).Trim() )
                        .FirstOrDefault( v => !string.IsNullOrEmpty( v ) );
                    if ( charset == null )
                    {
                        // Parse content-type too.
                        HtmlNode contentType = html.Descendants( "meta" ).FirstOrDefault( m => m.GetAttributeValue( "http-equiv" ) == "content-type" );
                        if ( contentType != null )
                        {
                            string contentTypeContent = contentType.GetAttributeValue( "content" );
                            int posContentType = contentTypeContent.IndexOf( "charset=" , StringComparison.Ordinal );
                            if ( posContentType != -1 )
                            {
                                charset = contentTypeContent.Substring( posContentType + "charset=".Length );
                            }
                        }
                    }

                    if ( !string.IsNullOrEmpty( charset ) )
                    {
                        Encoding = Encoding.GetEncoding( charset );
                        content = Encoding.GetString( rawResponse.Body );
                        html = content.ToHtmlNode();
                    }
                }
            }
            catch
            {

            }
        }

        public bool AutoDetectCharsetEncoding => autoDetectCharsetEncoding;

        public RawRequest RawRequest => rawRequest;

        public RawResponse RawResponse => rawResponse;

        public IEnumerable<HtmlNode> Find( string tag , By by )
        {
            return @by.CreateElementFinder( html , tag ).FindElements();
        }

        public IEnumerable<HyperLink> FindLinks( By by )
        {
            return Find( "a" , by ).Select( a => new HyperLink( this , a ) );
        }

        public PageWebForm FindForm( string name )
        {
            HtmlNode node = (from n in Html.Descendants( "form" )
                             let formName = n.GetAttributeValue( "name" , string.Empty )
                             where formName == name
                             select n).FirstOrDefault();

            return node == null ? null : new PageWebForm( node , browser );
        }

        public PageWebForm FindFormById( string id )
        {
            HtmlNode node = Html.Descendants( "form" ).FirstOrDefault( f => f.Id == id );
            return node == null ? null : new PageWebForm( node , browser );
        }

        public PageWebForm FindFormByAttribute( string attributeName , string attributeValue )
        {
            HtmlNode node = Html.Descendants( "form" )
                .FirstOrDefault( f => f.Attributes[attributeName] != null && 
                                      string.Equals(f.Attributes[attributeName].Value , attributeValue , StringComparison.InvariantCultureIgnoreCase));
            return node == null ? null : new PageWebForm( node , browser );
        }


        private void LoadBaseUrl()
        {
            string baseAttr = html.Descendants( "base" ).Where( e => e.Attributes.Any( a => a.Name == "href" ) )
                .Select( e => e.Attributes["href"].Value ).FirstOrDefault();

            if ( baseAttr != null )
            {
                baseUrl = baseAttr;
                return;
            }

            baseUrl = string.Format( "{0}://{1}" , absoluteUrl.Scheme , absoluteUrl.Host );
            if ( !absoluteUrl.IsDefaultPort )
            {
                baseUrl += ":" + absoluteUrl.Port;
            }
        }

        public override string ToString()
        {
            return content;
        }

        public static implicit operator string( WebPage page )
        {
            return page.content;
        }

        private void DownloadResources()
        {
            List<string> resourceUrls = GetResourceUrls();

            foreach ( string resourceUrl in resourceUrls )
            {
                Uri url = GetFullResourceUrl( resourceUrl , absoluteUrl );

                if ( WebResourceStorage.Current.Exists( url.ToString() ) )
                {
                    continue;
                }

                try
                {
                    WebResource resource = browser.DownloadWebResource( url );
                    resources.Add( resource );
                    if ( !resource.ForceDownload || !string.IsNullOrEmpty( resource.LastModified ) )
                    {
                        WebResourceStorage.Current.Save( resource );
                    }
                }
                catch
                {

                }
            }
        }

        private Uri GetFullResourceUrl( string resourceUrl , Uri root )
        {
            Uri.TryCreate( resourceUrl , UriKind.RelativeOrAbsolute , out Uri result );
            Uri url;

            if ( !result.IsAbsoluteUri )
            {
                if ( resourceUrl.StartsWith( "/" ) || resourceUrl.StartsWith( "./" ) || resourceUrl.StartsWith( "../" ) )
                {
                    url = baseUrl != null ? baseUrl.CombineUrl( resourceUrl ) : root.Combine( resourceUrl );
                }
                else
                {
                    string path = string.Join( "/" , root.Segments.Take( root.Segments.Length - 1 ).Skip( 1 ) );
                    url = baseUrl != null ? baseUrl.CombineUrl( path ).Combine( resourceUrl ) : root.Combine( resourceUrl );
                }
            }
            else
            {
                url = new Uri( resourceUrl );
            }

            return url;
        }

        public List<string> GetResourceUrls()
        {
            List<string> resourceUrls = new List<string>();

            foreach ( KeyValuePair<string , string> resourceTag in resourceTags )
            {
                string[] sources = html.Descendants( resourceTag.Key )
                    .Where( e => e.Attributes.Any( a => a.Name == resourceTag.Value ) )
                    .Select( e => e.Attributes[resourceTag.Value].Value ).ToArray();
                resourceUrls.AddRange( sources );
            }
            return resourceUrls;
        }

        public ScrapingBrowser Browser => browser;

        public Uri AbsoluteUrl => absoluteUrl;

        public string Content => content;

        public List<WebResource> Resources => resources;

        public HtmlNode Html => html;

        public string BaseUrl => baseUrl;

        private static readonly Regex urlInCssRegex = new Regex( @"url \s* [(] \s* (?<url>[^)\r\n]+) \s* [)]" ,
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace );

        public void SaveSnapshot( string path )
        {
            if ( !browser.AutoDownloadPagesResources )
            {
                DownloadResources();
            }

            if ( !Directory.Exists( path ) )
            {
                Directory.CreateDirectory( path );
            }
            else
            {
                Directory.GetFiles( path ).ToList().ForEach( File.Delete );
            }

            foreach ( WebResource resource in Resources )
            {
                Guid guid = Guid.NewGuid();
                resource.Content.Position = 0;
                string fileName = guid.ToString( "N" );

                RewriteHtml( resource , fileName );

                if ( !string.IsNullOrEmpty( resource.ContentType ) && resource.ContentType.EndsWith( "css" , StringComparison.InvariantCultureIgnoreCase ) )
                {
                    string textContent = resource.GetTextContent();
                    textContent = RewriteCssUrls( path , textContent , resource.AbsoluteUrl.ToString() );
                    File.WriteAllText( Path.Combine( path , fileName ) , textContent );
                }
                else
                {
                    File.WriteAllBytes( Path.Combine( path , fileName ) , resource.Content.ToArray() );
                }
            }

            string outerHtml = RewriteCssUrls( path , Html.OuterHtml , AbsoluteUrl.ToString() );
            File.WriteAllText( Path.Combine( path , "page.html" ) , outerHtml );
        }

        private void RewriteHtml( WebResource resource , string fileName )
        {
            foreach ( KeyValuePair<string , string> resourceTag in resourceTags )
            {
                HtmlNode[] nodes = html.Descendants( resourceTag.Key )
                    .Where(
                        e =>
                        e.Attributes.Any( a => a.Name == resourceTag.Value ) &&
                        resource.AbsoluteUrl.ToString().EndsWith( e.Attributes[resourceTag.Value].Value ) )
                    .ToArray();

                foreach ( HtmlNode node in nodes )
                {
                    node.SetAttributeValue( resourceTag.Value , fileName );
                }
            }
        }

        private string RewriteCssUrls( string path , string textContent , string rootUrl )
        {
            Match match = urlInCssRegex.Match( textContent );
            while ( match.Success )
            {
                string imageId = Guid.NewGuid().ToString( "N" );
                string url = match.Groups["url"].Value;

                string[] parts = rootUrl.Split( '/' );
                string leftPart = string.Join( "/" , parts.Take( parts.Length - 1 ) );

                try
                {
                    WebResource image = browser.DownloadWebResource( GetFullResourceUrl( url , new Uri( leftPart ) ) );
                    File.WriteAllBytes( Path.Combine( path , imageId ) , image.Content.ToArray() );
                }
                catch
                {

                }

                textContent = textContent.Replace( url , imageId );
                match = match.NextMatch();
            }
            return textContent;
        }
    }
}