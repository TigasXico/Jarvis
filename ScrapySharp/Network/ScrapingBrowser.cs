using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

using ScrapySharp.Extensions;

namespace ScrapySharp.Network
{
    public class ScrapingBrowser
    {
        private CookieContainer cookieContainer;
        private Uri referer;

        private static readonly Regex parseMetaRefreshRegex = new Regex( @"((?<seconds>[0-9]+);)?\s*URL=(?<url>(.+))" , RegexOptions.Compiled );

        public ScrapingBrowser()
        {
            InitCookieContainer();
            UserAgent = FakeUserAgents.Chrome24;
            AllowAutoRedirect = true;
            Language = CultureInfo.CreateSpecificCulture( "EN-US" );
            UseDefaultCookiesParser = true;
            IgnoreCookies = false;
            ProtocolVersion = HttpVersion.Version10;
            KeepAlive = false;
            Proxy = WebRequest.DefaultWebProxy;
            Headers = new Dictionary<string , string>();
            ClearHeadersAfterRequest = true;
            Encoding = Encoding.ASCII;
            AutoDetectCharsetEncoding = true;
        }

        public void ClearCookies()
        {
            InitCookieContainer();
        }

        private void InitCookieContainer()
        {
            cookieContainer = new CookieContainer();
        }

        public WebResource DownloadWebResource( Uri url )
        {
            WebResponse response = ExecuteRequest( url , HttpVerb.Get , new NameValueCollection() );
            MemoryStream memoryStream = new MemoryStream();
            Stream responseStream = response.GetResponseStream();

            if ( responseStream != null )
            {
                responseStream.CopyTo( memoryStream );
            }

            responseStream.Close();
            return new WebResource( memoryStream , response.Headers["Last-Modified"] , url , !IsCached( response.Headers["Cache-Control"] ) , response.ContentType );
        }

        private bool IsCached( string header )
        {
            if ( string.IsNullOrEmpty( header ) )
            {
                return false;
            }

            string[] noCacheHeaders = new[]
                {
                    "no-cache",
                    "no-store",
                    "max-age=0",
                    "pragma: no-cache",
                };

            return !noCacheHeaders.Contains( header.ToLowerInvariant() );
        }

        public string AjaxDownloadString( Uri url )
        {
            return AjaxDownloadStringAsync( url ).Result;
        }

        public async Task<string> AjaxDownloadStringAsync( Uri url )
        {
            HttpWebRequest request = CreateRequest( url , HttpVerb.Get );
            request.Headers["X-Prototype-Version"] = "1.6.1";
            request.Headers["X-Requested-With"] = "XMLHttpRequest";

            return await GetResponseAsync( url , request , 0 , new byte[0] );
        }

        public string DownloadString( Uri url )
        {
            return DownloadStringAsync( url ).Result;
        }

        public async Task<string> DownloadStringAsync( Uri url )
        {
            HttpWebRequest request = CreateRequest( url , HttpVerb.Get );
            return await GetResponseAsync( url , request , 0 , new byte[0] );
        }

        public Dictionary<string , string> Headers
        {
            get; private set;
        }

        public bool ClearHeadersAfterRequest
        {
            get; set;
        }

        public Encoding Encoding
        {
            get; set;
        }

        public bool AutoDetectCharsetEncoding
        {
            get; set;
        }

        public DecompressionMethods DecompressionMethods
        {
            get; set;
        }

        private HttpWebRequest CreateRequest( Uri url , HttpVerb verb )
        {
            HttpWebRequest request = ( HttpWebRequest ) WebRequest.Create( url.AbsoluteUri );
            request.Referer = referer?.AbsoluteUri;
            request.Method = ToMethod( verb );
            request.CookieContainer = cookieContainer;
            request.UserAgent = UserAgent.UserAgent;
            request.Proxy = Proxy;
            request.AutomaticDecompression = DecompressionMethods;

            request.Headers["Accept-Language"] = Language.Name;

            if ( Headers != null )
            {
                foreach ( KeyValuePair<string , string> header in Headers )
                {
                    if ( header.Key.ToLower() == "accept" )
                    {
                        request.Accept = header.Value;
                    }
                    else if ( header.Key.ToLower() == "referer" )
                    {
                        request.Referer = header.Value;
                    }
                    else
                    {
                        request.Headers[header.Key] = header.Value;
                    }
                }
                if ( ClearHeadersAfterRequest )
                {
                    Headers.Clear();
                }
            }

            request.CachePolicy = CachePolicy;

            if ( Timeout > TimeSpan.Zero )
            {
                request.Timeout = ( int ) Timeout.TotalMilliseconds;
            }

            request.KeepAlive = KeepAlive;
            request.ProtocolVersion = ProtocolVersion;

            if ( !string.IsNullOrWhiteSpace( TransferEncoding ) )
            {
                request.SendChunked = true;
                request.TransferEncoding = TransferEncoding;
            }
            else
            {
                request.SendChunked = SendChunked;
            }

            return request;
        }

        public bool SendChunked
        {
            get; set;
        }

        public IWebProxy Proxy
        {
            get; set;
        }

        public RequestCachePolicy CachePolicy
        {
            get; set;
        }

        public bool AllowMetaRedirect
        {
            get; set;
        }

        public bool AutoDownloadPagesResources
        {
            get; set;
        }

        private async Task<WebPage> GetResponseAsync( Uri url , HttpWebRequest request , int iteration , byte[] requestBody )
        {
            HttpWebResponse response = await GetWebResponseAsync( url , request ).ConfigureAwait( false );
            Stream responseStream = response.GetResponseStream();
            List<KeyValuePair<string , string>> headers = request.Headers.AllKeys.Select( k => new KeyValuePair<string , string>( k , request.Headers[k] ) ).ToList();

            if ( responseStream == null )
            {
                return new WebPage( this , url , AutoDownloadPagesResources ,
                    new RawRequest( request.Method , request.RequestUri , request.ProtocolVersion , headers , requestBody , Encoding ) ,
                    new RawResponse( response.ProtocolVersion , response.StatusCode , response.StatusDescription , response.Headers , new byte[0] , Encoding ) , Encoding , AutoDetectCharsetEncoding );
            }

            MemoryStream body = new MemoryStream();
            responseStream.CopyTo( body );
            responseStream.Close();
            body.Position = 0;
            string content = Encoding.GetString( body.ToArray() );
            body.Position = 0;

            RawRequest rawRequest = new RawRequest( request.Method , request.RequestUri , request.ProtocolVersion , headers , requestBody , Encoding );
            WebPage webPage = new WebPage( this , url , AutoDownloadPagesResources , rawRequest ,
                new RawResponse( response.ProtocolVersion , response.StatusCode , response.StatusDescription , response.Headers , body.ToArray() , Encoding ) , Encoding , AutoDetectCharsetEncoding );

            if ( AllowMetaRedirect && !string.IsNullOrEmpty( response.ContentType ) && response.ContentType.Contains( "html" ) && iteration < 10 )
            {
                HtmlAgilityPack.HtmlNode html = content.ToHtmlNode();
                HtmlAgilityPack.HtmlNode meta = html.CssSelect( "meta" )
                    .FirstOrDefault( p => p.Attributes != null && p.Attributes.HasKeyIgnoreCase( "HTTP-EQUIV" )
                                          && p.Attributes.GetIgnoreCase( "HTTP-EQUIV" ).Equals( "refresh" , StringComparison.InvariantCultureIgnoreCase ) );

                if ( meta != null )
                {
                    string attr = meta.Attributes.GetIgnoreCase( "content" );
                    Match match = parseMetaRefreshRegex.Match( attr );
                    if ( !match.Success )
                    {
                        return webPage;
                    }

                    int seconds = 0;
                    if ( match.Groups["seconds"].Success )
                    {
                        seconds = int.Parse( match.Groups["seconds"].Value );
                    }

                    if ( !match.Groups["url"].Success )
                    {
                        return webPage;
                    }

                    string redirect = Unquote( match.Groups["url"].Value );

                    if ( !Uri.TryCreate( redirect , UriKind.RelativeOrAbsolute , out Uri redirectUrl ) )
                    {
                        return webPage;
                    }

                    if ( !redirectUrl.IsAbsoluteUri )
                    {
                        string baseUrl = string.Format( "{0}://{1}" , url.Scheme , url.Host );
                        if ( !url.IsDefaultPort )
                        {
                            baseUrl += ":" + url.Port;
                        }

                        if ( redirect.StartsWith( "/" ) )
                        {
                            redirectUrl = baseUrl.CombineUrl( redirect );
                        }
                        else
                        {
                            string path = string.Join( "/" , url.Segments.Take( url.Segments.Length - 1 ).Skip( 1 ) );
                            redirectUrl = baseUrl.CombineUrl( path ).Combine( redirect );
                        }
                    }

                    await Task.Delay( TimeSpan.FromSeconds( seconds ) );

                    return await DownloadRedirect( redirectUrl , iteration + 1 );
                }
            }

            return webPage;
        }

        private string Unquote( string value )
        {
            if ( string.IsNullOrEmpty( value ) )
            {
                return value;
            }

            if ( value.StartsWith( "'" ) || value.StartsWith( "\"" ) )
            {
                value = value.Substring( 1 );
            }

            if ( value.EndsWith( "'" ) || value.EndsWith( "\"" ) && value.Length > 1 )
            {
                value = value.Substring( 0 , value.Length - 1 );
            }

            return value;
        }

        private async Task<WebPage> DownloadRedirect( Uri url , int iteration )
        {
            HttpWebRequest request = CreateRequest( url , HttpVerb.Get );
            return await GetResponseAsync( url , request , iteration , new byte[0] );
        }

        public string TransferEncoding
        {
            get; set;
        }

        private async Task<HttpWebResponse> GetWebResponseAsync( Uri url , HttpWebRequest request )
        {
            referer = url;
            request.AllowAutoRedirect = AllowAutoRedirect;
            HttpWebResponse response;
            try
            {
                response = ( HttpWebResponse ) await request.GetResponseAsync().ConfigureAwait( false );
            }
            catch ( WebException e )
            {
                response = ( HttpWebResponse ) e.Response;
            }

            WebHeaderCollection headers = response.Headers;

            if ( !IgnoreCookies )
            {
                string cookiesExpression = headers["Set-Cookie"];
                if ( !string.IsNullOrEmpty( cookiesExpression ) )
                {
                    Uri cookieUrl =
                        new Uri( string.Format( "{0}://{1}:{2}/" , response.ResponseUri.Scheme , response.ResponseUri.Host ,
                                              response.ResponseUri.Port ) );
                    if ( UseDefaultCookiesParser )
                    {
                        cookieContainer.SetCookies( cookieUrl , cookiesExpression );
                    }
                    else
                    {
                        SetCookies( url , cookiesExpression );
                    }
                }
            }
            return response;
        }

        public void SetCookies( Uri cookieUrl , string cookiesExpression )
        {
            CookiesParser parser = new CookiesParser( cookieUrl.Host );
            List<Cookie> cookies = parser.ParseCookies( cookiesExpression );
            CookieCollection previousCookies = cookieContainer.GetCookies( cookieUrl );

            foreach ( Cookie cookie in cookies )
            {
                Cookie c = previousCookies[cookie.Name];
                if ( c != null )
                {
                    c.Value = cookie.Value;
                }
                else
                {
                    cookieContainer.Add( cookie );
                }
            }
        }

        public WebResponse ExecuteRequest( Uri url , HttpVerb verb , NameValueCollection data )
        {
            return ExecuteRequest( url , verb , GetHttpPostVars( data ) );
        }
        public async Task<WebResponse> ExecuteRequestAsync( Uri url , HttpVerb verb , NameValueCollection data )
        {
            return await ExecuteRequestAsync( url , verb , GetHttpPostVars( data ) );
        }

        public WebResponse ExecuteRequest( Uri url , HttpVerb verb , string data )
        {
            return ExecuteRequestAsync( url , verb , data ).Result;
        }

        public async Task<WebResponse> ExecuteRequestAsync( Uri url , HttpVerb verb , string data , string contentType = null )
        {
            string path = string.IsNullOrEmpty( data )
                              ? url.AbsoluteUri
                              : (verb == HttpVerb.Get ? string.Format( "{0}?{1}" , url.AbsoluteUri , data ) : url.AbsoluteUri);

            HttpWebRequest request = CreateRequest( new Uri( path ) , verb );

            if ( verb == HttpVerb.Post )
            {
                request.ContentType = contentType ?? "application/x-www-form-urlencoded";
            }

            request.CookieContainer = cookieContainer;

            if ( verb == HttpVerb.Post )
            {
                Stream stream = await request.GetRequestStreamAsync();
                using ( StreamWriter writer = new StreamWriter( stream ) )
                {
                    writer.Write( data );
                    writer.Flush();
                }
            }

            return await GetWebResponseAsync( url , request );
        }

        public string NavigateTo( Uri url , HttpVerb verb , string data )
        {
            return NavigateToPage( url , verb , data );
        }

        public WebPage NavigateToPage( Uri url , HttpVerb verb = HttpVerb.Get , string data = "" , string contentType = null )
        {
            Task<WebPage> task = NavigateToPageAsync( url , verb , data , contentType );
            task.Wait();
            return task.Result;
        }

        public async Task<WebPage> NavigateToPageAsync( Uri url , HttpVerb verb = HttpVerb.Get , string data = "" , string contentType = null )
        {
            string path = string.IsNullOrEmpty( data )
                              ? url.AbsoluteUri
                              : (verb == HttpVerb.Get ? string.Format( "{0}?{1}" , url.AbsoluteUri , data ) : url.AbsoluteUri);

            HttpWebRequest request = CreateRequest( new Uri( path ) , verb );

            if ( verb == HttpVerb.Post )
            {
                request.ContentType = contentType ?? "application/x-www-form-urlencoded";
            }

            request.CookieContainer = cookieContainer;

            if ( verb == HttpVerb.Post )
            {
                Stream stream = await request.GetRequestStreamAsync().ConfigureAwait( false );
                using ( StreamWriter writer = new StreamWriter( stream ) )
                {
                    writer.Write( data );
                    writer.Flush();
                }
            }

            return await GetResponseAsync( url , request , 0 , Encoding.GetBytes( data ) ).ConfigureAwait( false );
        }

        public WebPage NavigateToPage( Uri url , HttpVerb verb , NameValueCollection data )
        {
            return NavigateToPage( url , verb , GetHttpPostVars( data ) );
        }

        public string NavigateTo( Uri url , HttpVerb verb , NameValueCollection data )
        {
            return NavigateTo( url , verb , GetHttpPostVars( data ) );
        }

        private static string ToMethod( HttpVerb verb )
        {
            switch ( verb )
            {
                case HttpVerb.Get:
                    return "GET";
                case HttpVerb.Head:
                    return "HEAD";
                case HttpVerb.Post:
                    return "POST";
                case HttpVerb.Put:
                    return "PUT";
                case HttpVerb.Delete:
                    return "DELETE";
                case HttpVerb.Trace:
                    return "TRACE";
                case HttpVerb.Options:
                    return "OPTIONS";
                default:
                    throw new ArgumentOutOfRangeException( "verb" );
            }
        }

        public static string GetHttpPostVars( NameValueCollection variables )
        {
            StringBuilder builder = new StringBuilder();

            for ( int i = 0 ; i < variables.Count ; i++ )
            {
                string key = variables.GetKey( i );
                string[] values = variables.GetValues( i );
                if ( values != null )
                {
                    foreach ( string value in values )
                    {
                        builder.AppendFormat( "{0}={1}" , HttpUtility.UrlEncode( key ) , HttpUtility.UrlEncode( value ) );
                    }
                }

                if ( i < variables.Count - 1 )
                {
                    builder.Append( "&" );
                }
            }

            return builder.ToString();
        }

        public FakeUserAgent UserAgent
        {
            get; set;
        }

        public bool AllowAutoRedirect
        {
            get; set;
        }

        public bool UseDefaultCookiesParser
        {
            get; set;
        }

        public bool IgnoreCookies
        {
            get; set;
        }

        public TimeSpan Timeout
        {
            get; set;
        }

        public CultureInfo Language
        {
            get; set;
        }

        public Version ProtocolVersion
        {
            get; set;
        }

        public bool KeepAlive
        {
            get; set;
        }

        public Uri Referer => referer;

        public Cookie GetCookie( Uri url , string name )
        {
            CookieCollection collection = cookieContainer.GetCookies( url );

            return collection[name];
        }

        public CookieCollection GetCookieCollection( Uri url )
        {
            return cookieContainer.GetCookies( url );
        }
    }
}
