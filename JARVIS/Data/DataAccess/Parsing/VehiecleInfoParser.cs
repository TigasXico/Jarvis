using System;
using System.Linq;
using HtmlAgilityPack;
using Jarvis.Data.DataModels;
using Jarvis.Services;
using ScrapySharp.Extensions;

namespace Jarvis.Data.DataAccess.Parsing
{
    public static class VehiecleInfoParser
    {
        public static bool ParseEntityVehieclesInfo( HtmlNode vehiecleInfoAsHtml , FiscalEntityDataModel fiscalEntity )
        {
            try
            {
                lock ( fiscalEntity.updatingCollectionsLock )
                {
                    fiscalEntity.Vehiecles.Clear();
                }

                var allListedVehiecles = vehiecleInfoAsHtml.CssSelect( "#tabela" ).CssSelect( "tr" ).ToList();

                foreach ( var currentVehiecle in allListedVehiecles )
                {
                    var currentVehiecleInfo = currentVehiecle.CssSelect( "td" ).ToArray();

                    if ( currentVehiecleInfo.Length != 0 )
                    {
                        lock ( fiscalEntity.updatingCollectionsLock )
                        {
                            var clientVehiecle = new VehiecleDataModel()
                            {
                                Owner = fiscalEntity ,
                                RoleOfClient = ParsingUtils.GetFieldValueClean( currentVehiecleInfo , 4 ) ,
                                LicensePlate = ParsingUtils.GetFieldValueClean( currentVehiecleInfo , 0 ) ,
                                Brand = ParsingUtils.GetFieldValueClean( currentVehiecleInfo , 2 ) ,
                                Model = ParsingUtils.GetFieldValueClean( currentVehiecleInfo , 3 ) ,
                            };

                            var dateOfLicensePlateAsString = ParsingUtils.GetFieldValueClean( currentVehiecleInfo , 1 );

                            if ( !string.IsNullOrWhiteSpace( dateOfLicensePlateAsString ) )
                            {
                                clientVehiecle.DateOfLicensePlate = DateTime.Parse( dateOfLicensePlateAsString ).Date;
                            }

                            fiscalEntity.Vehiecles.Add( clientVehiecle );
                        }
                    }
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