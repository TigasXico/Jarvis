using System;
using HtmlAgilityPack;
using Jarvis.Controllers.Contract;
using ScrapySharp.Network;

namespace Jarvis.Data.Contract
{
    public interface IWebScraper<T> : IDisposable
    {
        bool IsLoggedIn
        {
            get;
        }

        ScrapingBrowser InitScrapper();

        OperationResult LoginEntity( T entityToLogIn );

        bool GetPage( Uri uri , out WebPage targetPage );

        bool GetPageContent( Uri url, out HtmlNode targetwebPage );

        bool GetPageAndContent( Uri uri , out WebPage targetPage , out HtmlNode targetwebPage );

        void LogOutEntity();
    }
}
