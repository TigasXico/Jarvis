using Jarvis.Data.Contract;

namespace Jarvis.Data.DataAccess.Scraping
{
    public abstract class WebScraperFactory<T>
    {
        protected abstract IWebScraper<T> MakeScraper();

        public IWebScraper<T> GetScraper()
        {
            return MakeScraper();
        }
    }
}
