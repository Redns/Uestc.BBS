using System;
using System.Linq;
using System.Reflection;
using FastEnumUtility;
using Microsoft.UI.Xaml.Data;

namespace Uestc.BBS.WinUI.Converters
{
    public partial class Enum2LabelConverter : IValueConverter
    {
        /// <summary>
        /// TODO 使用源码生成器代替
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language) =>
            value is Enum enumValue
            && enumValue
                .GetType()
                .GetMember(enumValue.ToString())
                .FirstOrDefault()
                ?.GetCustomAttribute<LabelAttribute>()
                ?.Value
                is string label
                ? label
                : $"Unknow value: {value}";

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
