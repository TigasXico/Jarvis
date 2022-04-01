
using Jarvis.DataAccess.WebScraping;
using Jarvis.Data.DataModels;
using Jarvis.Interfaces;

namespace Jarvis.DataAccess.WebScrapping
{
    public class FinancesWebScraperFactory : WebScraperFactory<FiscalEntityDataModel>
    {
        protected override IWebScraper<FiscalEntityDataModel> MakeScraper()
        {
            return new FinancesWebScraper();
        }
    }
}
