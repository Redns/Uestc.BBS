using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Uestc.BBS.Desktop.Converters
{
    /// <summary>
    /// 将日期转换为其与当前时刻的差值
    /// </summary>
    public class DateToRelativeDateStringConverter : IValueConverter
    {
        public static DateToRelativeDateStringConverter Instance { get; } = new();

        public object? Convert(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture
        )
        {
            if (value is DateTime date)
            {
                var timespan = DateTime.Now - date;
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

                return date.ToShortTimeString();
            }

            throw new ArgumentException(
                "Converter requires a DateTime type parameter",
                nameof(value)
            );
        }

        public object? ConvertBack(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture
        )
        {
            throw new NotImplementedException();
        }
    }
}
