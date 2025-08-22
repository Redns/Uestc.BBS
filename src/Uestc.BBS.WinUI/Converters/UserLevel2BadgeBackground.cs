using System;
using System.Linq;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Uestc.BBS.Core;
using Windows.UI;

namespace Uestc.BBS.WinUI.Converters
{
    /// <summary>
    /// 根据用户等级生成对应的徽标背景颜色
    /// </summary>
    public partial class UserLevel2BadgeBackground : IValueConverter
    {
        private static readonly Brush[] UserLevelBadgeBackgroundBrushes =
        [
            .. GlobalValues.UserLevelBadgeBackgroundBrushes.Select(b => new SolidColorBrush(
                Color.FromArgb((byte)(b >> 24), (byte)(b >> 16), (byte)(b >> 8), (byte)b)
            )),
        ];

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not uint level)
            {
                return UserLevelBadgeBackgroundBrushes[0];
            }

            if (level >= UserLevelBadgeBackgroundBrushes.Length)
            {
                return UserLevelBadgeBackgroundBrushes[^1];
            }

            return UserLevelBadgeBackgroundBrushes[level];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
