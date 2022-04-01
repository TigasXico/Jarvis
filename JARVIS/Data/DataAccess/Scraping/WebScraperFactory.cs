using Jarvis.Interfaces;

namespace Jarvis.DataAccess.WebScraping
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
