using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.FileCache;
using Uestc.BBS.Core.Services.System;

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
        /// 日志服务
        /// </summary>
        private static readonly ILogService _logService =
            ServiceExtension.Services.GetRequiredService<ILogService>();

        /// <summary>
        /// CancellationTokenSource
        /// </summary>
        private static readonly DependencyProperty CancellationTokenSourceProperty =
            DependencyProperty.RegisterAttached(
                "CancellationTokenSource",
                typeof(CancellationTokenSource),
                typeof(ImageCacheHelper),
                new PropertyMetadata(null)
            );

        public static CancellationTokenSource GetCancellationTokenSource(DependencyObject obj) =>
            (CancellationTokenSource)obj.GetValue(CancellationTokenSourceProperty);

        public static void SetCancellationTokenSource(
            DependencyObject obj,
            CancellationTokenSource value
        ) => obj.SetValue(CancellationTokenSourceProperty, value);

        /// <summary>
        /// 对外暴露的 AttachedProperty
        /// </summary>
        private static readonly DependencyProperty SourceExProperty =
            DependencyProperty.RegisterAttached(
                "SourceEx",
                typeof(string),
                typeof(ImageCacheHelper),
                new PropertyMetadata(null, OnSourceExChanged)
            );

        public static string GetSourceEx(Image obj) => (string)obj.GetValue(SourceExProperty);

        public static void SetSourceEx(Image obj, string sourceEx) =>
            obj.SetValue(SourceExProperty, sourceEx);

        private static async void OnSourceExChanged(
            DependencyObject obj,
            DependencyPropertyChangedEventArgs args
        )
        {
            if (obj is not Image image || args.NewValue is not string sourceEx)
            {
                return;
            }

            try
            {
                var oldCancellationTokenSource = GetCancellationTokenSource(obj);
                oldCancellationTokenSource?.Cancel();
                oldCancellationTokenSource?.Dispose();

                // 大图像加载流程尚未完成时，将 sourceEx 置空以取消
                if (string.IsNullOrEmpty(sourceEx))
                {
                    return;
                }

                var newCancellationTokenSource = new CancellationTokenSource();
                obj.SetValue(CancellationTokenSourceProperty, new CancellationTokenSource());

                var imageUri = await _fileCache.GetFileUriAsync(
                    new Uri(sourceEx),
                    newCancellationTokenSource.Token
                );

                image.Source = new BitmapImage(imageUri);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                _logService.Error($"Image source ({sourceEx}) is invalid", ex);
            }
        }
    }
}
