using System;
using System.Linq;
using HtmlAgilityPack;
using Jarvis.Data.DataModels;
using Jarvis.Services;
using ScrapySharp.Extensions;

namespace Jarvis.Data.DataAccess.Parsing
{
    public static class ClientBasicInfoParser 
    {
        public static bool ParseClientBasicInfo( HtmlNode clientInfoAsHtml , ref ClientDataModel client )
        {
            try
            {
                var allFieldValues = clientInfoAsHtml.CssSelect( "dd" ).ToArray();

                client.Name = ParsingUtils.GetFieldValueClean( allFieldValues , 1 );
                client.BirthDate = ParsingUtils.GetFieldValueClean( allFieldValues , 2 );
                client.Gender = ParsingUtils.GetFieldValueClean( allFieldValues , 3 );
                client.Nationality = ParsingUtils.GetFieldValueClean( allFieldValues , 8 );
                client.FinancialServicesRepartition = ParsingUtils.GetFieldValueClean(allFieldValues, 9);
                client.FiscalAddress = ParsingUtils.GetFieldValueClean( allFieldValues , 14 );
                client.FiscalAddressZipCode = ParsingUtils.GetFieldValueClean( allFieldValues , 16 );

                var locality = ParsingUtils.GetFieldValueClean(allFieldValues, 19);
                var council = ParsingUtils.GetFieldValueClean(allFieldValues, 18);
                var district = ParsingUtils.GetFieldValueClean(allFieldValues, 17);

                client.FiscalAddressAdditionalInfo = $"{locality} , {council} - {district}";

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
