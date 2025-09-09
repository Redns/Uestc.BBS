using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Uestc.BBS.Sdk;

namespace Uestc.BBS.WinUI.Converters
{
    public partial class Uid2Space : DependencyObject, IValueConverter
    {
        private static readonly DependencyProperty BaseUriProperty =
            DependencyProperty.RegisterAttached(
                nameof(BaseUri),
                typeof(Uri),
                typeof(Uid2Avatar),
                new PropertyMetadata(ApiEndpoints.BaseUri)
            );

        public Uri BaseUri
        {
            get => (Uri)GetValue(BaseUriProperty);
            set => SetValue(BaseUriProperty, value);
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not uint uid || uid is 0)
            {
                return BaseUri.AbsoluteUri;
            }

            return BaseUri.AbsoluteUri + "user/" + uid;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
