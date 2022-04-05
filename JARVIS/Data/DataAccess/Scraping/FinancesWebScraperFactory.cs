using Jarvis.Data.Contract;
using Jarvis.Data.DataModels;

namespace Jarvis.Data.DataAccess.Scraping
{
    public class FinancesWebScraperFactory : WebScraperFactory<FiscalEntityDataModel>
    {
        protected override IWebScraper<FiscalEntityDataModel> MakeScraper()
        {
            return new FinancesWebScraper();
        }
    }
}
