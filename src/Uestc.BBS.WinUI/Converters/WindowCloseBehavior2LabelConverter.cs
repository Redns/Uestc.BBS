using System;
using FastEnumUtility;
using Microsoft.UI.Xaml.Data;
using Uestc.BBS.Core;

namespace Uestc.BBS.WinUI.Converters
{
    public partial class WindowCloseBehavior2LabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not WindowCloseBehavior behavior)
            {
                throw new ArgumentException("Value is not WindowCloseBehavior", nameof(value));
            }

            return behavior.GetLabel()
                ?? throw new ArgumentException("Value must have a label", nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
