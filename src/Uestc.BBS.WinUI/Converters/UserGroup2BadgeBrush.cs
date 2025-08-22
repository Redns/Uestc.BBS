using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Uestc.BBS.Core;
using Windows.UI;

namespace Uestc.BBS.WinUI.Converters
{
    public partial class UserGroup2BadgeBrush : IValueConverter
    {
        private static readonly Dictionary<string, SolidColorBrush> UserGroupBadgeBackgroundBrushs =
            GlobalValues.UserVisibleGroups.ToDictionary(
                g => g.Key,
                g => new SolidColorBrush(
                    Color.FromArgb(
                        (byte)(g.Value >> 24),
                        (byte)(g.Value >> 16),
                        (byte)(g.Value >> 8),
                        (byte)(g.Value)
                    )
                )
            );

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not string group)
            {
                throw new ArgumentException(
                    "Cannot convert user group to badge background brush, value is not a string"
                );
            }

            if (UserGroupBadgeBackgroundBrushs.TryGetValue(group, out var brush))
            {
                return brush;
            }

            return UserGroupBadgeBackgroundBrushs.First().Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
