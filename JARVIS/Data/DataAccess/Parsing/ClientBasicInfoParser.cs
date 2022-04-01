using HtmlAgilityPack;

using Jarvis.Data.DataModels;
using Jarvis.Services;

using ScrapySharp.Extensions;

using System;
using System.Linq;

namespace Jarvis.DataAccess.Parsers
{
    public static class ClientBasicInfoParser 
    {
        public static bool ParseClientBasicInfo( HtmlNode clientInfoAsHtml , ref ClientDataModel client )
        {
            try
            {

                HtmlNode[] allFieldValues = clientInfoAsHtml.CssSelect( ".fieldValue" ).ToArray();

                client.Name = ParsingUtils.GetFieldValueClean( allFieldValues , 1 );
                client.BirthDate = ParsingUtils.GetFieldValueClean( allFieldValues , 2 );
                client.Gender = ParsingUtils.GetFieldValueClean( allFieldValues , 3 );
                client.Nationality = ParsingUtils.GetFieldValueClean( allFieldValues , 8 );
                client.FiscalAddress = ParsingUtils.GetFieldValueClean( allFieldValues , 9 );
                client.FiscalAddressAdditionalInfo = ParsingUtils.GetFieldValueClean( allFieldValues , 10 );
                client.FiscalAddressZipCode = ParsingUtils.GetFieldValueClean( allFieldValues , 11 );
                client.FinancialServicesRepartition = ParsingUtils.GetFieldValueClean( allFieldValues , 16 );

                return true;
            }
            catch (Exception ex)
            {
                WindowService.ShowException( ex );
                return false;
            }
        }
    }
}
