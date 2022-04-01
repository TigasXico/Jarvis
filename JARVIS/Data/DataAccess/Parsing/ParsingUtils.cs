using HtmlAgilityPack;

using ScrapySharp.Extensions;


namespace Jarvis.DataAccess.Parsers
{
    public abstract class ParsingUtils
    {
        internal static string GetFieldValueClean( HtmlNode[] array , int index )
        {
            if ( index >= 0 && index < array.Length )
            {
                HtmlNode node = array[index];
                return ClearString( node.InnerText );
            }
            else
            {
                return null;
            }
        }

        protected static string ClearString( string textToClean )
        {
            string cleanedString = textToClean.Replace( "\\r\\n" , string.Empty );
            cleanedString = cleanedString.Replace( "\\t" , string.Empty );
            cleanedString = cleanedString.Trim();
            return cleanedString;
        }

        internal static bool TryGetValidAttributeValue( HtmlNode field , string attributeName , out string attributeValue )
        {
            attributeValue = null;

            if ( field != null && field != default( HtmlNode ) && field.HasAttributes && field.Attributes.HasKeyIgnoreCase( attributeName ) )
            {
                attributeValue = field.Attributes.GetIgnoreCase( attributeName );
                return !string.IsNullOrWhiteSpace( attributeValue );
            }

            return false;

        }
    }
}