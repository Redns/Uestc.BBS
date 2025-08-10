using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.FileCache;

namespace Uestc.BBS.WinUI.Helpers
{
    public class ImageCacheHelper
    {
        /// <summary>
        /// 文件缓存服务
        /// </summary>
        private static readonly IFileCache _fileCache =
            ServiceExtension.Services.GetRequiredService<IFileCache>();

        /// <summary>
        /// 对外暴露的 AttachedProperty
        /// </summary>
        public static readonly DependencyProperty SourceExProperty =
            DependencyProperty.RegisterAttached(
                "SourceEx",
                typeof(Uri),
                typeof(ImageCacheHelper),
                new PropertyMetadata(null, OnSourceExChanged)
            );

        public static Uri GetSourceEx(Image obj) => (Uri)obj.GetValue(SourceExProperty);

        public static void SetSourceEx(Image obj, Uri sourceEx) =>
            obj.SetValue(SourceExProperty, sourceEx);

        private static async void OnSourceExChanged(
            DependencyObject obj,
            DependencyPropertyChangedEventArgs args
        )
        {
            if (obj is not Image image || args.NewValue is not Uri uri)
            {
                return;
            }
            image.Source = new BitmapImage(await _fileCache.GetFileUriAsync(uri));
        }
    }
}
