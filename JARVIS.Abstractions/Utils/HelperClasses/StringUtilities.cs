using System;

namespace Jarvis.Utils.HelperClasses
{
    public static class StringUtilities
    {
        public static bool StartsWith( this string source , string[] possibleStarts , StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase )
        {
            foreach ( string possibleStart in possibleStarts )
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
    }
}
