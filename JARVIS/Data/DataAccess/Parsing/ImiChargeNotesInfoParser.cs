
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using HtmlAgilityPack;

using Jarvis.Data.DataModels;
using log4net;

using ScrapySharp.Extensions;

namespace Jarvis.Data.DataAccess.Parsing
{
    public static class ImiChargeNotesInfoParser
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ImiChargeNotesInfoParser));

        internal static bool GetInfoOfImiPaymentOffYear(HtmlNode paymentInfoAsHtml, FiscalEntityDataModel fiscalEntity)
        {
            try
            {
                //Get year of search
                var yearCell = paymentInfoAsHtml.CssSelect(".bTD").Skip(1).SingleOrDefault();

                if (yearCell == default)
                {
                    logger.Error("Cannot parse IMI Payment - cannot obtain year from details page");
                    return false;
                }

                var year = yearCell.InnerText;

                //Get payment details
                var payments = paymentInfoAsHtml.CssSelect(".iTR").Skip(2);

                lock (fiscalEntity.updatingCollectionsLock)
                {

                    fiscalEntity.ImiChargeNotes.Clear();

                    foreach (var currentPaymentDetails in payments)
                    {
                        var currentPaymentInfo = currentPaymentDetails.CssSelect(".iTD").ToArray();

                        if (currentPaymentInfo.Length != 0)
                        {
                            var chargeNoteStatusInt =
                                int.Parse(ParsingUtils.GetFieldValueClean(currentPaymentInfo, 3).Substring(0, 1));

                            var chargeNoteStatus = (ImiChargeNoteStatus) (chargeNoteStatusInt);

                            var fieldValue = ParsingUtils.GetFieldValueClean(currentPaymentInfo, 1);

                            var paymentValueAsString = Regex.Replace(fieldValue, @"[^0-9,.]", "");

                            var paymentValue = decimal.Parse(paymentValueAsString);

                            var limitDate = DateTime.Parse(ParsingUtils.GetFieldValueClean(currentPaymentInfo, 2));

                            var numberOfBuildings = int.Parse(ParsingUtils.GetFieldValueClean(currentPaymentInfo, 5));

                            var currentPaymentDetail = new ImiChargeNotesDataModel
                            {
                                ChargeNoteNumber = ParsingUtils.GetFieldValueClean(currentPaymentInfo, 0),
                                PaymentValue = paymentValue,
                                LimitDate = limitDate,
                                Status = chargeNoteStatus,
                                PaymentReference = ParsingUtils.GetFieldValueClean(currentPaymentInfo, 4),
                                NumberOfBuildings = numberOfBuildings,
                                Year = year
                            };

                            fiscalEntity.ImiChargeNotes.Add(currentPaymentDetail);
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
