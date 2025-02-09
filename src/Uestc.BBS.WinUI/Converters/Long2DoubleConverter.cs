using System;
using Microsoft.UI.Xaml.Data;

namespace Uestc.BBS.WinUI.Converters
{
    public partial class Long2DoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) =>
            value is long l ? (double)l : 0;

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            string language
        ) => value is double d ? (long)d : 0;
    }
}
