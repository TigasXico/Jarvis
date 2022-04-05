using System;
using System.Text;

namespace Jarvis.Utils.HelperClasses
{
    public static class StringUtilities
    {
        public static bool StartsWith( this string source , string[] possibleStarts , StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase )
        {
            foreach ( var possibleStart in possibleStarts )
            {
                if ( source.StartsWith( possibleStart , stringComparison ) )
                {
                    return true;
                }
            }

            return false;
        }

        public static bool Contains( this string source , string substring , StringComparison comparisonMode )
        {
            return source?.IndexOf( substring , comparisonMode ) >= 0;
        }

        public static string ToReadableException( this Exception e )
        {
            var result = new StringBuilder();
            result.AppendLine( $"Exception occured({e.GetType()}) - {e.Message}");
            result.AppendLine( $"StackTrace: {e.StackTrace}" );

            if ( e.InnerException != null )
            {
                result.AppendLine( " ------------------ Inner Exception ------------------ " );
                result.AppendLine( ToReadableException( e.InnerException ) );
            }

            return result.ToString();
        }
    }
}
