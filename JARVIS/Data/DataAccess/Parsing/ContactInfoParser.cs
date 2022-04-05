using System;
using System.Linq;
using HtmlAgilityPack;
using Jarvis.Data.DataModels;
using Jarvis.Services;
using log4net;
using ScrapySharp.Extensions;

namespace Jarvis.Data.DataAccess.Parsing
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

                    var emailField = contactInfoAsHtml.CssSelect( "#email" ).FirstOrDefault();

                    if ( ParsingUtils.TryGetValidAttributeValue( emailField , "value" , out var email ) )
                    {
                        var phoneContact = new ContactDataModel
                        {
                            ContactHolder = fiscalEntity ,
                            ContactType = ContactType.Email ,
                            ContactValue = email
                        };

                        fiscalEntity.Contacts.Add( phoneContact );

                    }

                    #endregion

                    #region Get phone contact

                    var phoneContactField = contactInfoAsHtml.CssSelect( "#telefone" ).FirstOrDefault();

                    if ( ParsingUtils.TryGetValidAttributeValue( phoneContactField , "value" , out var phone ) )
                    {
                        var phoneContact = new ContactDataModel
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
