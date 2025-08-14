using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace Uestc.BBS.WinUI.Converters
{
    public partial class Null2Visibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return
                parameter is not null
                && bool.TryParse(parameter.ToString(), out bool reverse)
                && reverse
                ? value is null
                    ? Visibility.Visible
                    : Visibility.Collapsed
                : value is null
                    ? Visibility.Collapsed
                    : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
