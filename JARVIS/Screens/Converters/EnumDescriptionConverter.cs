using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace Jarvis.Screens.Converters
{
    public class EnumDescriptionConverter : IValueConverter
    {
        private string GetEnumDescription( Enum enumObj )
        {
            if ( enumObj == null )
            {
                return string.Empty;
            }

            var fieldInfo = enumObj.GetType().GetField( enumObj.ToString() );

            var attribArray = fieldInfo.GetCustomAttributes( false );

            if ( attribArray.Length == 0 )
            {
                return enumObj.ToString();
            }
            else
            {
                var attrib = attribArray[0] as DescriptionAttribute;
                return attrib.Description;
            }
        }

        public object Convert( object value , Type targetType , object parameter , CultureInfo culture )
        {
            var myEnum = ( Enum ) value;

            if ( myEnum == null )
            {
                return null;
            }
            var description = GetEnumDescription( myEnum );

            if ( !string.IsNullOrEmpty( description ) )
            {
                return description;
            }

            return myEnum.ToString();
        }

        public object ConvertBack( object value , Type targetType , object parameter , CultureInfo culture )
        {
            return string.Empty;
        }
    }
}