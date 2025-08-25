using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Uestc.BBS.Core.Models;

namespace Uestc.BBS.WinUI.Converters
{
    public partial class MenuKey2BadgeVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not MenuItemKey key)
            {
                return Visibility.Collapsed;
            }

            return key is MenuItemKey.Messages ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
