using System;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Uestc.BBS.Core;

namespace Uestc.BBS.WinUI.Converters
{
    public partial class UserGroup2Visibility : IValueConverter
    {
        /// <summary>
        /// 判断是否显示用户组
        /// </summary>
        /// <param name="value">用户组</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">返回 bool/Visibility（默认 Visibility）</param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var isUserGroupVisiable =
                value is string group && GlobalValues.UserVisibleGroups.Any(g => g.Key == group);
            return parameter is "True" ? isUserGroupVisiable
                : isUserGroupVisiable ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
