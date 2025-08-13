using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace Uestc.BBS.WinUI.Converters
{
    public partial class Null2Visibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is not null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
