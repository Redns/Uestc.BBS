using System;
using Microsoft.UI.Xaml.Data;
using Uestc.BBS.Core.Helpers;

namespace Uestc.BBS.WinUI.Converters
{
    public partial class Date2RelativeStringConverter : IValueConverter
    {
        /// <summary>
        /// 将主题时间转换为相对时间字符串
        /// </summary>
        /// <param name="value">热门主题为发表时间，其余主题为最新回复时间</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">是否为热门主题</param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not DateTime date)
            {
                throw new ArgumentException("Convert value must be a DateTime", nameof(value));
            }

            return parameter is not "True"
                ? date.ToRelativeDateString()
                : date.Year == DateTime.Now.Year
                    ? date.ToString("MM/dd HH:mm")
                    : date.ToShortDateString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
