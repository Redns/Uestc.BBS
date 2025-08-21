using System;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace Uestc.BBS.WinUI.Converters
{
    public partial class UserLevel2BadgeBackground : IValueConverter
    {
        private static readonly Brush[] UserLevelBadgeBackgroundBrushes =
        [
            new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x67, 0xC0)), // Lv.0
            new SolidColorBrush(Color.FromArgb(0xFF, 0x7D, 0x7D, 0x7D)), // Lv.1  灰
            new SolidColorBrush(Color.FromArgb(0xFF, 0xEE, 0x78, 0x00)), // Lv.2  橙
            new SolidColorBrush(Color.FromArgb(0xFF, 0xEC, 0x68, 0x00)), // Lv.3
            new SolidColorBrush(Color.FromArgb(0xFF, 0xEC, 0x6D, 0x51)), // Lv.4
            new SolidColorBrush(Color.FromArgb(0xFF, 0xD3, 0xA2, 0x43)), // Lv.5  黄
            new SolidColorBrush(Color.FromArgb(0xFF, 0xD9, 0xA6, 0x2E)), // Lv.6
            new SolidColorBrush(Color.FromArgb(0xFF, 0xE6, 0xB4, 0x22)), // Lv.7
            new SolidColorBrush(Color.FromArgb(0xFF, 0x3E, 0xB3, 0x70)), // Lv.8  绿
            new SolidColorBrush(Color.FromArgb(0xFF, 0x02, 0x87, 0x60)), // Lv.9
            new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x55, 0x2E)), // Lv.10
            new SolidColorBrush(Color.FromArgb(0xFF, 0x38, 0xB4, 0x8B)), // Lv.11 青
            new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0xA3, 0x81)), // Lv.12
            new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x6E, 0x54)), // Lv.13
            new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x95, 0xD9)), // Lv.14 蓝
            new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x94, 0xC8)), // Lv.15
            new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x7B, 0xBB)), // Lv.16
            new SolidColorBrush(Color.FromArgb(0xFF, 0x74, 0x53, 0x99)), // Lv.17 紫
            new SolidColorBrush(Color.FromArgb(0xFF, 0x67, 0x45, 0x98)), // Lv.18
            new SolidColorBrush(Color.FromArgb(0xFF, 0x65, 0x31, 0x8E)), // Lv.19
        ];

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not uint level || level >= UserLevelBadgeBackgroundBrushes.Length)
            {
                return UserLevelBadgeBackgroundBrushes[0];
            }

            return UserLevelBadgeBackgroundBrushes[level];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
