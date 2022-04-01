using System.Linq;
using System.Reflection;

namespace Jarvis.Utils.HelperClasses
{
    public static class AutoMapper
    {
        public static T2 ConvertToDerived<T1, T2>( this T1 baseObj ) where T2 : new()
        {
            T2 derivedObj = new T2();
            PropertyInfo[] sourceProperties = baseObj.GetType().GetProperties();
            PropertyInfo[] targetProperties = derivedObj.GetType().GetProperties();

            foreach ( PropertyInfo baseProperty in sourceProperties )
            {
                PropertyInfo targetProperty = targetProperties.SingleOrDefault( property => property.Name.Equals( baseProperty.Name ) );

                if ( targetProperty != default && baseProperty.CanRead && targetProperty.CanWrite )
                {
                    object val = baseProperty.GetValue( baseObj );

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
