using System;
using Microsoft.UI.Xaml.Data;

namespace Uestc.BBS.WinUI.Converters
{
    public partial class Uint2DoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) =>
            value is uint u ? (double)u : 0;

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            string language
        ) => value is double d ? (uint)d : 0;
    }
}
