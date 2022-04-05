using System.Linq;
using System.Reflection;

namespace Jarvis.Utils.HelperClasses
{
    public static class AutoMapper
    {
        public static T2 ConvertToDerived<T1, T2>( this T1 baseObj ) where T2 : new()
        {
            var derivedObj = new T2();
            var sourceProperties = baseObj.GetType().GetProperties();
            var targetProperties = derivedObj.GetType().GetProperties();

            foreach ( var baseProperty in sourceProperties )
            {
                var targetProperty = targetProperties.SingleOrDefault( property => property.Name.Equals( baseProperty.Name ) );

                if ( targetProperty != default && baseProperty.CanRead && targetProperty.CanWrite )
                {
                    var val = baseProperty.GetValue( baseObj );

                    if ( val != null )
                    {
                        targetProperty.SetValue( derivedObj , val );
                    }
                }
            }

            return derivedObj;
        }
    }
}
