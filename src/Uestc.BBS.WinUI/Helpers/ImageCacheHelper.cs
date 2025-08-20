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

        private static CancellationTokenSource GetCancellationTokenSource(DependencyObject obj) =>
            (CancellationTokenSource)obj.GetValue(CancellationTokenSourceProperty);

        private static void SetCancellationTokenSource(
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

        /// <summary>
        /// 智能拉伸
        /// 当 Image 设置 MaxHeight/MaxWidth 时，如果图像的实际 Height/Widht 比设置的 MaxHeight/MaxWidth 小，则保持原图尺寸；
        /// 当 Image 设置 MinHeight/MinWidth 时，如果图像的实际 Height/Widht 比设置的 MinHeight/MinWidth 大，则保持原图尺寸；
        /// 否则，根据图像的实际尺寸自动调整图像的拉伸方式。
        /// </summary>
        private static readonly DependencyProperty SmartStretchProperty =
            DependencyProperty.RegisterAttached(
                "SmartStretch",
                typeof(bool),
                typeof(ImageCacheHelper),
                new PropertyMetadata(false)
            );

        public static bool GetSmartStretch(Image obj) => (bool)obj.GetValue(SmartStretchProperty);

        public static void SetSmartStretch(Image obj, bool value) =>
            obj.SetValue(SmartStretchProperty, value);

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
                var bitmapImage = new BitmapImage(imageUri);
                bitmapImage.ImageOpened += (_, __) =>
                {
                    if (!GetSmartStretch(image))
                    {
                        return;
                    }

                    if (
                        image.MaxHeight != double.PositiveInfinity
                        || image.MaxWidth != double.PositiveInfinity
                    )
                    {
                        if (
                            bitmapImage.PixelHeight <= image.MaxHeight
                            && bitmapImage.PixelWidth <= image.MaxWidth
                        )
                        {
                            image.Width = bitmapImage.PixelWidth;
                            image.Height = bitmapImage.PixelHeight;
                            return;
                        }
                    }

                    if (image.MinHeight != 0 || image.MinWidth != 0)
                    {
                        if (
                            bitmapImage.PixelHeight >= image.MinHeight
                            && bitmapImage.PixelWidth >= image.MinWidth
                        )
                        {
                            image.Width = bitmapImage.PixelWidth;
                            image.Height = bitmapImage.PixelHeight;
                            return;
                        }
                    }
                };

                image.Source = bitmapImage;
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                _logService.Error($"Image source ({sourceEx}) is invalid", ex);
            }
        }

        /// <summary>
        /// 图像懒加载
        /// </summary>
        /// <param name="element"></param>
        /// <param name="args"></param>
        public static void ImageLazyLoad(
            FrameworkElement element,
            EffectiveViewportChangedEventArgs args
        )
        {
            if (element is not Image image)
            {
                return;
            }

            // 当图像距离视窗底部小于两倍窗口高度时，开始加载图像
            // 此处实际上应该使用 ScrollViewer 的高度，但 ScrollViewer 高度不确定，
            // 由于首页显示主题列表的 ScrollViewer 高度与窗口近似，故使用 App.CurrentWindow.Bounds.Height
            if (args.BringIntoViewDistanceY < App.CurrentWindow?.Bounds.Height * 2)
            {
                image.EffectiveViewportChanged -= ImageLazyLoad;
                SetSourceEx(image, image.Tag as string ?? string.Empty);
            }
        }
    }
}
