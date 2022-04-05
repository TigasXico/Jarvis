using System.Net;
using HtmlAgilityPack;
using ScrapySharp.Extensions;

namespace Jarvis.Data.DataAccess.Parsing
{
    public abstract class ParsingUtils
    {
        internal static string GetFieldValueClean( HtmlNode[] array , int index )
        {
            if ( index >= 0 && index < array.Length )
            {
                var node = array[index];
                return ClearString( node.InnerText );
            }
            else
            {
                return null;
            }
        }

        protected static string ClearString( string textToClean )
        {
            var cleanedString = textToClean.Replace( "\\r\\n" , string.Empty );
            cleanedString = cleanedString.Replace( "\\t" , string.Empty );
            cleanedString = cleanedString.Trim();

            cleanedString = WebUtility.HtmlDecode(cleanedString);

            return cleanedString;
        }

        internal static bool TryGetValidAttributeValue( HtmlNode field , string attributeName , out string attributeValue )
        {
            attributeValue = null;

            if (field == default || !field.HasAttributes || !field.Attributes.HasKeyIgnoreCase(attributeName))
            {
                return false;
            }

            attributeValue = field.Attributes.GetIgnoreCase( attributeName );

            return !string.IsNullOrWhiteSpace( attributeValue );

        }
    }
}