using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Uestc.BBS.Core.Helpers;

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
            if (value is not DateTime date)
            {
                throw new ArgumentException(
                    "Converter requires a DateTime type parameter",
                    nameof(value)
                );
            }

            return date.ToRelativeDateString();
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
