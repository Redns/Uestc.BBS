using System;
using FastEnumUtility;
using Microsoft.UI.Xaml.Data;
using Uestc.BBS.Core;

namespace Uestc.BBS.WinUI.Converters
{
    public partial class ThemeColor2LabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not ThemeColor themeColor)
            {
                throw new ArgumentException("Value must be an enum type", nameof(value));
            }

            return themeColor.GetLabel()
                ?? throw new ArgumentException("Value must have a label", nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
