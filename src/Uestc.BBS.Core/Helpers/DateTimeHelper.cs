using System.Globalization;

namespace Uestc.BBS.Core.Helpers
{
    public static class DateTimeHelper
    {
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
                _ => throw new ArgumentOutOfRangeException(nameof(dayOfWeek))
            };

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
            var startOfLastWeek = startOfWeek.Subtract(TimeSpan.FromDays(7));

            if (date >= startOfWeek)
            {
                return "周" + date.DayOfWeek.GetDayOfWeekInChinese();
            }

            if (date >= startOfLastWeek && date < startOfWeek)
            {
                return "上周" + date.DayOfWeek.GetDayOfWeekInChinese();
            }

            return date.ToShortDateString();
        }
    }
}
