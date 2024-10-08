﻿using System;

using HtmlAgilityPack;

using Jarvis.Data.DataModels;
using Jarvis.Interfaces;

using ScrapySharp.Network;

namespace Jarvis.DataAccess.WebScraping
{
    public abstract class FiscalEntityWebScraper : IWebScraper<FiscalEntityDataModel>
    {
        private ScrapingBrowser scrapingBrowser;
        protected ScrapingBrowser ScrapingBrowser
        {
            get
            {
                if ( scrapingBrowser == null )
                {
                    scrapingBrowser = InitScrapper();
                }

                return scrapingBrowser;
            }

            set => scrapingBrowser = value;
        }

        public abstract bool IsLoggedIn
        {
            get;
            protected set;
        }

        public abstract ScrapingBrowser InitScrapper();

        public abstract OperationResult LoginEntity( FiscalEntityDataModel entityToLogIn );
        
        public abstract bool GetPage( Uri uri , out WebPage targetPage );

        public abstract bool GetPageContent( Uri Url , out HtmlNode targetwebPage );

        public abstract bool GetPageAndContent( Uri uri , out WebPage targetPage , out HtmlNode targetwebPage );

        public abstract void LogOutEntity();

        public void Dispose()
        {
            LogOutEntity();
        }
    }
}
