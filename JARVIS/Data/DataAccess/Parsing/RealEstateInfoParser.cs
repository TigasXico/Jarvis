using HtmlAgilityPack;

using Jarvis.Data.DataModels;
using Jarvis.Services;

using Newtonsoft.Json;

using ScrapySharp.Extensions;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;

namespace Jarvis.DataAccess.Parsers
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
                HtmlNode scriptWithRealEstateInfo = contactInfoAsHtml.CssSelect( "script" ).Last();

                Match match = Regex.Match( scriptWithRealEstateInfo.InnerText , RealEstateExtractionRegEx );

                string content = match.Groups["Content"].Value;

                List<Dictionary<string , string>> listOfRealEstates = JsonConvert.DeserializeObject<List<Dictionary<string , string>>>( content );

                if ( listOfRealEstates != null )
                {
                    lock ( fiscalEntity.updatingCollectionsLock )
                    {

                        fiscalEntity.RealEstates.Clear();

                        foreach ( Dictionary<string , string> realEstate in listOfRealEstates )
                        {
                            RealEstateDataModel currentRealEstate = new RealEstateDataModel()
                            {
                                Location = realEstate["loc"] ,
                                FullArticle = realEstate["artM"] ,
                                Part = realEstate["qP"] ,
                                MatrixYear = (int.TryParse( realEstate["ano"] , out int result ) ? result : (( int? ) (null))) ,
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
