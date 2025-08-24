using System.Globalization;

namespace Uestc.BBS.Core.Helpers
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// 获取星期几的中文名称
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string GetDayOfWeekInChinese(this DayOfWeek dayOfWeek) =>
            dayOfWeek switch
            {
                DayOfWeek.Monday => "一",
                DayOfWeek.Tuesday => "二",
                DayOfWeek.Wednesday => "三",
                DayOfWeek.Thursday => "四",
                DayOfWeek.Friday => "五",
                DayOfWeek.Saturday => "六",
                DayOfWeek.Sunday => "日",
                _ => throw new ArgumentOutOfRangeException(nameof(dayOfWeek)),
            };

        /// <summary>
        /// 获取相对时间字符串
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToRelativeDateString(this DateTime date)
        {
            var now = DateTime.Now;
            var timespan = now - date;

            if (timespan < TimeSpan.FromMinutes(1))
            {
                return "刚刚";
            }

            if (timespan < TimeSpan.FromHours(1))
            {
                return (uint)timespan.TotalMinutes + " 分钟前";
            }

            if (timespan < TimeSpan.FromDays(1))
            {
                return (uint)timespan.TotalHours + " 小时前";
            }

            if (timespan < TimeSpan.FromDays(2))
            {
                return "昨天";
            }

            if (timespan < TimeSpan.FromDays(3))
            {
                return "前天";
            }

            var startOfWeek = now.Date.Subtract(
                TimeSpan.FromDays(
                    now.DayOfWeek - CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek
                )
            );

            if (date >= startOfWeek)
            {
                return "周" + date.DayOfWeek.GetDayOfWeekInChinese();
            }

            if (date.Year == now.Year)
            {
                return date.ToString("MM/dd HH:mm");
            }

            return date.ToString("yyyy/MM/dd HH:mm");
        }
    }
}
