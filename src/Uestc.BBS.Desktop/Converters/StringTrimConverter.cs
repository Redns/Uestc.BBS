using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Uestc.BBS.Desktop.Converters
{
    public class StringTrimConverter : IValueConverter
    {
        public static StringTrimConverter Instance { get; } = new();

        public object? Convert(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture
        )
        {
            if (value is not string str)
            {
                throw new ArgumentException(
                    "Converter requires a string type parameter",
                    nameof(value)
                );
            }

            return str.Trim();
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
