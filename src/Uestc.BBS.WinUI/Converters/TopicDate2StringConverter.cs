using System;
using Microsoft.UI.Xaml.Data;
using Uestc.BBS.Core.Helpers;

namespace Uestc.BBS.WinUI.Converters
{
    public partial class TopicDate2StringConverter : IValueConverter
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="value">热门帖子为发表时间，其余帖子为最新回复时间</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">是否为热门帖子</param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not DateTime date)
            {
                throw new ArgumentException("Topic date convert failed", nameof(value));
            }

            return parameter is true
                ? date.ToRelativeDateString()
                : $"{date.ToRelativeDateString()}有回复";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
