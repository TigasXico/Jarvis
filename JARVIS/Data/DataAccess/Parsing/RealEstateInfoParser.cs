using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Jarvis.Data.DataModels;
using Jarvis.Services;
using Newtonsoft.Json;
using ScrapySharp.Extensions;

namespace Jarvis.Data.DataAccess.Parsing
{
    public static class RealEstateInfoParser
    {
        #region Script info extraction related configs

        private const string realEstateExtractionRegExConfigKey = "RealEstateExtractionRegEx";
        private static string RealEstateExtractionRegEx => ConfigurationManager.AppSettings.Get( realEstateExtractionRegExConfigKey );

        #endregion

        public static bool ParseEntityRealEstatetInfo( HtmlNode contactInfoAsHtml , FiscalEntityDataModel fiscalEntity )
        {
            try
            {
                var scriptWithRealEstateInfo = contactInfoAsHtml.CssSelect( "script" ).Last();

                var match = Regex.Match( scriptWithRealEstateInfo.InnerText , RealEstateExtractionRegEx );

                var content = match.Groups["Content"].Value;

                var listOfRealEstates = JsonConvert.DeserializeObject<List<Dictionary<string , string>>>( content );

                if ( listOfRealEstates != null )
                {
                    lock ( fiscalEntity.updatingCollectionsLock )
                    {
                        fiscalEntity.RealEstates.Clear();

                        foreach ( var realEstate in listOfRealEstates )
                        {
                            var currentRealEstate = new RealEstateDataModel()
                            {
                                Location = realEstate["loc"] ,
                                FullArticle = realEstate["artM"] ,
                                Part = realEstate["qP"] ,
                                MatrixYear = (int.TryParse( realEstate["ano"] , out var result ) ? result : (( int? ) (null))) ,
                                InitialValue = realEstate["vIni"] ,
                                CurrentValue = realEstate["val"] ,
                            };

                            fiscalEntity.RealEstates.Add( currentRealEstate );
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
