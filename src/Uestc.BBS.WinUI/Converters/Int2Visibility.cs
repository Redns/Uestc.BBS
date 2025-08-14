using System;
using System.Text.RegularExpressions;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace Uestc.BBS.WinUI.Converters
{
    public partial class Int2Visibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not int num)
            {
                return Visibility.Collapsed;
            }

            if (parameter is not string comparerAndValue)
            {
                return num > 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            var comparerAndValueRegex = ComparerAndValueRegex().Match(comparerAndValue);
            if (comparerAndValueRegex.Groups.Count != 2)
            {
                return num > 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            if (int.TryParse(comparerAndValueRegex.Groups[2].Value, out var compareValue))
            {
                return num > 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            return comparerAndValueRegex.Groups[1].Value switch
            {
                ">" => num > compareValue,
                ">=" => num >= compareValue,
                "<" => num < compareValue,
                "<=" => num <= compareValue,
                "==" => num == compareValue,
                "!=" => num != compareValue,
                _ => false,
            }
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        [GeneratedRegex(@"^(>=|<=|>|<|==|!=)(\d+)$")]
        private static partial Regex ComparerAndValueRegex();
    }
}
