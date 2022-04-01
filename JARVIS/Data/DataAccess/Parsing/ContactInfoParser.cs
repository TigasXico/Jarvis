using HtmlAgilityPack;

using Jarvis.Data.DataModels;
using Jarvis.Services;

using log4net;

using ScrapySharp.Extensions;

using System;
using System.Linq;

namespace Jarvis.DataAccess.Parsers
{
    public static class ContactInfoParser
    {
        private static readonly ILog logger = LogManager.GetLogger( typeof( ContactInfoParser ) );

        public static bool ParseEntityContactInfo( HtmlNode contactInfoAsHtml , FiscalEntityDataModel fiscalEntity )
        {

            try
            {
                lock ( fiscalEntity.updatingCollectionsLock )
                {
                    fiscalEntity.Contacts.Clear();

                    #region Get email address

                    HtmlNode emailField = contactInfoAsHtml.CssSelect( "#email" ).FirstOrDefault();

                    if ( ParsingUtils.TryGetValidAttributeValue( emailField , "value" , out string email ) )
                    {
                        ContactDataModel phoneContact = new ContactDataModel
                        {
                            ContactHolder = fiscalEntity ,
                            ContactType = ContactType.Email ,
                            ContactValue = email
                        };

                        fiscalEntity.Contacts.Add( phoneContact );

                    }

                    #endregion

                    #region Get phone contact

                    HtmlNode phoneContactField = contactInfoAsHtml.CssSelect( "#telefone" ).FirstOrDefault();

                    if ( ParsingUtils.TryGetValidAttributeValue( phoneContactField , "value" , out string phone ) )
                    {
                        ContactDataModel phoneContact = new ContactDataModel
                        {
                            ContactHolder = fiscalEntity ,
                            ContactType = ContactType.PhoneNumber ,
                            ContactValue = phone
                        };

                        fiscalEntity.Contacts.Add( phoneContact );
                    }

                    #endregion
                }

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
