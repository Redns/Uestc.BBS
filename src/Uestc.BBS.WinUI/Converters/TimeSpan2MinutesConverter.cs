using System;
using Microsoft.UI.Xaml.Data;

namespace Uestc.BBS.WinUI.Converters
{
    public partial class TimeSpan2MinutesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not TimeSpan timeSpan)
            {
                throw new ArgumentException("value is not a TimeSpan", nameof(value));
            }

            return timeSpan.TotalMinutes;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is not double minutes)
            {
                throw new ArgumentException("value is not a double", nameof(value));
            }

            return TimeSpan.FromMinutes(minutes);
        }
    }
}
