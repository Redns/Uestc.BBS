using System;
using System.Linq;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Uestc.BBS.WinUI.Converters
{
    public partial class StringArray2ImageSourceArray : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not string[] imageUrls)
            {
                throw new ArgumentException(
                    "Convert value to ImageSource[] failed, value is not string[]",
                    nameof(value)
                );
            }
            return imageUrls.Select(imageUrl => new BitmapImage(new Uri(imageUrl))).ToArray();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
