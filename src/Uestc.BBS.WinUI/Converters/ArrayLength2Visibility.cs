using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace Uestc.BBS.WinUI.Converters
{
    public partial class ArrayLength2Visibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is null)
            {
                return Visibility.Collapsed;
            }

            if (value is not Array array)
            {
                throw new ArgumentException(
                    "Convert value to visibility failed, value is not a Array",
                    nameof(value)
                );
            }

            return
                int.TryParse((string)parameter, out int lengthParameter)
                || array.Length >= lengthParameter
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
