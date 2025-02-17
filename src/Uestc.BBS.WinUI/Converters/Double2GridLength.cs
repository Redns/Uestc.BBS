using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace Uestc.BBS.WinUI.Converters
{
    public partial class Double2GridLength : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not double doubleValue)
            {
                throw new ArgumentException("Value is not a double", nameof(value));
            }

            return new GridLength(doubleValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is not GridLength gridLength)
            {
                throw new ArgumentException("Value is not a GridLength", nameof(value));
            }

            return gridLength.Value;
        }
    }
}
