using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using Avalonia.Data.Converters;

namespace Uestc.BBS.Desktop.Converters
{
    public class ObservableCollectionIsNotEmptyConverter<T> : IValueConverter
    {
        public static ObservableCollectionIsNotEmptyConverter<T> Instance { get; } = new();

        public object? Convert(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture
        )
        {
            if (value is ObservableCollection<T> observableCollection)
            {
                return observableCollection.Count > 0;
            }

            if (value is Task<ObservableCollection<T>> observableCollectionTask)
            {
                return observableCollectionTask.Result.Count > 0;
            }

            throw new ArgumentException(
                "Converter requires a ObservableCollection type parameter",
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
