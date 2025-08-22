using System;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Uestc.BBS.Core;

namespace Uestc.BBS.WinUI.Converters
{
    public partial class UserGroup2Visibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not string group)
            {
                return Visibility.Collapsed;
            }

            return GlobalValues.UserVisibleGroups.Any(g => g.Key == group)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
