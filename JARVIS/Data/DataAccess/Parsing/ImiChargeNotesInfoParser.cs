
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using HtmlAgilityPack;

using Jarvis.Data.DataModels;
using Jarvis.DataAccess.Parsers;

using log4net;

using ScrapySharp.Extensions;

namespace Jarvis.Data.DataAccess.Parsing
{
    public static class ImiChargeNotesInfoParser
    {
        private static readonly ILog logger = LogManager.GetLogger( typeof( ImiChargeNotesInfoParser ) );

        internal static bool GetInfoOfImiPaymentOffYear( HtmlNode paymentInfoAsHtml , FiscalEntityDataModel fiscalEntity )
        {
            try
            {
                //Get year of search
                HtmlNode yearCell = paymentInfoAsHtml.CssSelect( ".bTD" ).Skip( 1 ).SingleOrDefault();

                if ( yearCell == default )
                {
                    logger.Error( "Cannot parse IMI Payment - cannot obtain year from details page" );
                    return false;
                }

                string year = yearCell.InnerText;

                //Get payment details
                IEnumerable<HtmlNode> payments = paymentInfoAsHtml.CssSelect( ".iTR" ).Skip( 2 );

                foreach ( HtmlNode currentPaymentDetails in payments )
                {
                    HtmlNode[] currentPaymentInfo = currentPaymentDetails.CssSelect( ".iTD" ).ToArray();

                    if ( currentPaymentInfo.Length != 0 )
                    {
                        int chargeNoteStatusInt = int.Parse( ParsingUtils.GetFieldValueClean( currentPaymentInfo , 3 ).Substring( 0 , 1 ) );

                        ImiChargeNoteStatus chargeNoteStatus = ( ImiChargeNoteStatus ) (chargeNoteStatusInt);

                        string fieldValue = ParsingUtils.GetFieldValueClean( currentPaymentInfo , 1 );

                        string paymentValueAsString = Regex.Replace( fieldValue , @"[^0-9,.]" , "" );

                        decimal paymentValue = decimal.Parse( paymentValueAsString );

                        DateTime limitDate = DateTime.Parse( ParsingUtils.GetFieldValueClean( currentPaymentInfo , 2 ) );

                        int numberOfBuildings = int.Parse( ParsingUtils.GetFieldValueClean( currentPaymentInfo , 5 ));

                        ImiChargeNotesDataModel currentPaymentDetail = new ImiChargeNotesDataModel
                        {
                            ChargeNoteNumber = ParsingUtils.GetFieldValueClean( currentPaymentInfo , 0 ) ,
                            PaymentValue =  paymentValue,
                            LimitDate = limitDate ,
                            Status = chargeNoteStatus ,
                            PaymentReference = ParsingUtils.GetFieldValueClean( currentPaymentInfo , 4 ) ,
                            NumberOfBuildings = numberOfBuildings,
                            Year = year
                        };

                        lock ( fiscalEntity.updatingCollectionsLock )
                        {
                            fiscalEntity.ImiChargeNotes.Add( currentPaymentDetail );
                        }
                    }
                }

                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
