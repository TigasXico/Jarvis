using System;
using System.Linq;
using HtmlAgilityPack;
using Jarvis.Data.DataModels;
using Jarvis.Services;
using ScrapySharp.Extensions;

namespace Jarvis.Data.DataAccess.Parsing
{
    public static class CompanyBasicInfoParser
    {
        public static bool ParseCompanyBasicInfo( HtmlNode clientInfoAsHtml , ref CompanyDataModel company )
        {
            try
            {
                var allFieldValues = clientInfoAsHtml.CssSelect( ".fieldValue" ).ToArray();

                company.Name = ParsingUtils.GetFieldValueClean( allFieldValues , 1 );
                company.FiscalAddress = ParsingUtils.GetFieldValueClean( allFieldValues , 2 );
                company.FiscalAddressZipCode = ParsingUtils.GetFieldValueClean( allFieldValues , 4 );
                company.FiscalAddressAdditionalInfo = $" {ParsingUtils.GetFieldValueClean( allFieldValues , 7 )} , {ParsingUtils.GetFieldValueClean( allFieldValues , 6 )} - {ParsingUtils.GetFieldValueClean( allFieldValues , 5 )}";
                company.FinancialServicesRepartition = ParsingUtils.GetFieldValueClean( allFieldValues , 9 );

                return true;
            }
            catch ( Exception ex )
            {
                WindowService.ShowException( ex );
                return false;
            }
        }
    }
}
