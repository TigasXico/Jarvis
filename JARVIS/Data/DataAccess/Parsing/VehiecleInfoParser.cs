using System;
using System.Collections.Generic;
using System.Linq;

using HtmlAgilityPack;

using Jarvis.Data.DataModels;
using Jarvis.Services;

using ScrapySharp.Extensions;

namespace Jarvis.DataAccess.Parsers
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

                List<HtmlNode> allListedVehiecles = vehiecleInfoAsHtml.CssSelect( "#tabela" ).CssSelect( "tr" ).ToList();

                foreach ( HtmlNode currentVehiecle in allListedVehiecles )
                {
                    HtmlNode[] currentVehiecleInfo = currentVehiecle.CssSelect( "td" ).ToArray();

                    if ( currentVehiecleInfo.Length != 0 )
                    {
                        lock ( fiscalEntity.updatingCollectionsLock )
                        {
                            VehiecleDataModel clientVehiecle = new VehiecleDataModel()
                            {
                                Owner = fiscalEntity ,
                                RoleOfClient = ParsingUtils.GetFieldValueClean( currentVehiecleInfo , 4 ) ,
                                LicensePlate = ParsingUtils.GetFieldValueClean( currentVehiecleInfo , 0 ) ,
                                Brand = ParsingUtils.GetFieldValueClean( currentVehiecleInfo , 2 ) ,
                                Model = ParsingUtils.GetFieldValueClean( currentVehiecleInfo , 3 ) ,
                            };

                            string dateOfLicensePlateAsString = ParsingUtils.GetFieldValueClean( currentVehiecleInfo , 1 );

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