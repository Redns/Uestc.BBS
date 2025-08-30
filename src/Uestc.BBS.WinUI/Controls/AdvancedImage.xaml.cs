using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Services.FileCache;
using Uestc.BBS.Core.Services.System;

namespace Uestc.BBS.WinUI.Controls
{
    public partial class AdvancedImage : UserControl
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
        /// 图片源
        /// </summary>
        private static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            nameof(Source),
            typeof(string),
            typeof(AdvancedImage),
            new PropertyMetadata(
                null,
                static async (obj, args) =>
                {
                    if (obj is not AdvancedImage image || args.NewValue is not string source)
                    {
                        return;
                    }

                    if (image.Status is ImageStatus.Idle || image.Status is ImageStatus.Loading)
                    {
                        return;
                    }

                    await SetSourceAsync(image, source);
                }
            )
        );

        public string Source
        {
            get => (string)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        /// <summary>
        /// 拉伸状态
        /// </summary>
        private static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
            nameof(Stretch),
            typeof(Stretch),
            typeof(AdvancedImage),
            new PropertyMetadata(Stretch.None)
        );

        public Stretch Stretch
        {
            get => (Stretch)GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }

        /// <summary>
        /// 图片数据源
        /// </summary>
        private static readonly DependencyProperty SuccessImageSourceProperty =
            DependencyProperty.Register(
                nameof(SuccessImageSource),
                typeof(ImageSource),
                typeof(AdvancedImage),
                new PropertyMetadata(null)
            );

        private ImageSource SuccessImageSource
        {
            get => (ImageSource)GetValue(SuccessImageSourceProperty);
            set => SetValue(SuccessImageSourceProperty, value);
        }

        /// <summary>
        /// 是否启用延时加载（默认关闭）
        /// XXX 虚拟化和延时加载冲突，只能启用其中一个
        /// </summary>
        private static readonly DependencyProperty IsLazyLoadEnableProperty =
            DependencyProperty.Register(
                nameof(IsLazyLoadEnable),
                typeof(bool),
                typeof(AdvancedImage),
                new PropertyMetadata(false)
            );

        public bool IsLazyLoadEnable
        {
            get => (bool)GetValue(IsLazyLoadEnableProperty);
            set => SetValue(IsLazyLoadEnableProperty, value);
        }

        /// <summary>
        /// 是否启用缓存
        /// </summary>
        private static readonly DependencyProperty IsCachedEnableProperty =
            DependencyProperty.Register(
                nameof(IsCachedEnable),
                typeof(bool),
                typeof(AdvancedImage),
                new PropertyMetadata(true)
            );

        public bool IsCachedEnable
        {
            get => (bool)GetValue(IsCachedEnableProperty);
            set => SetValue(IsCachedEnableProperty, value);
        }

        /// <summary>
        /// 图片状态
        /// </summary>
        private static readonly DependencyProperty StatusProperty = DependencyProperty.Register(
            nameof(Status),
            typeof(ImageStatus),
            typeof(AdvancedImage),
            new PropertyMetadata(ImageStatus.Idle)
        );

        private ImageStatus Status
        {
            get => (ImageStatus)GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }

        #region 占位符相关属性
        /// <summary>
        /// 是否显示占位符
        /// </summary>
        private static readonly DependencyProperty ShowPlaceholderProperty =
            DependencyProperty.Register(
                nameof(ShowPlaceholder),
                typeof(bool),
                typeof(AdvancedImage),
                new PropertyMetadata(true)
            );

        public bool ShowPlaceholder
        {
            get => (bool)GetValue(ShowPlaceholderProperty);
            set => SetValue(ShowPlaceholderProperty, value);
        }

        /// <summary>
        /// 占位符宽度
        /// </summary>
        private static readonly DependencyProperty PlaceholderWidthProperty =
            DependencyProperty.Register(
                nameof(PlaceholderWidth),
                typeof(double),
                typeof(AdvancedImage),
                new PropertyMetadata(800.0)
            );

        public double PlaceholderWidth
        {
            get => (double)GetValue(PlaceholderWidthProperty);
            set => SetValue(PlaceholderWidthProperty, value);
        }

        /// <summary>
        /// 占位符高度
        /// </summary>
        private static readonly DependencyProperty PlaceholderHeightProperty =
            DependencyProperty.Register(
                nameof(PlaceholderHeight),
                typeof(double),
                typeof(AdvancedImage),
                new PropertyMetadata(600.0)
            );

        public double PlaceholderHeight
        {
            get => (double)GetValue(PlaceholderHeightProperty);
            set => SetValue(PlaceholderHeightProperty, value);
        }

        #endregion

        public AdvancedImage()
        {
            InitializeComponent();

            // 右键清除缓存
            RightTapped += async (sender, e) =>
            {
                if (sender is not AdvancedImage image)
                {
                    return;
                }

                if (!image.IsCachedEnable || image.Status is not ImageStatus.Success)
                {
                    return;
                }

                if (image.SuccessImageSource is not BitmapImage bitmapImage)
                {
                    return;
                }

                File.Delete(bitmapImage.UriSource.LocalPath);

                await SetSourceAsync(image, image.Source, true);
            };
            // 懒加载
            EffectiveViewportChanged += ImageLazyLoad;
        }

        private static async Task SetSourceAsync(
            AdvancedImage image,
            string source,
            bool ignoreCache = false
        )
        {
            if (string.IsNullOrEmpty(source))
            {
                return;
            }

            image.Status = ImageStatus.Loading;

            try
            {
                var imageUri = image.IsCachedEnable
                    ? await _fileCache
                        .GetFileUriAsync(new Uri(source))
                        .TimeoutCancelAsync(TimeSpan.FromMinutes(1))
                    : new Uri(source);
                image.SuccessImageSource = new BitmapImage(imageUri)
                {
                    DecodePixelHeight =
                        image.Height is not double.PositiveInfinity ? (int)image.Height
                        : image.MaxHeight is not double.PositiveInfinity ? (int)image.MaxHeight
                        : 0,
                    DecodePixelType = DecodePixelType.Logical,
                    CreateOptions = ignoreCache
                        ? BitmapCreateOptions.IgnoreImageCache
                        : BitmapCreateOptions.None,
                };

                image.Status = ImageStatus.Success;
            }
            catch (TaskCanceledException)
            {
                image.Status = ImageStatus.Error;
            }
            catch (Exception ex)
            {
                image.Status = ImageStatus.Error;

                _logService.Error($"Failed to load image {source}", ex);
            }
        }

        /// <summary>
        /// 图像懒加载
        /// </summary>
        /// <param name="element"></param>
        /// <param name="args"></param>
        public static async void ImageLazyLoad(
            FrameworkElement element,
            EffectiveViewportChangedEventArgs args
        )
        {
            if (element is not AdvancedImage image)
            {
                return;
            }

            if (image.Status is not ImageStatus.Idle)
            {
                return;
            }

            if (!image.IsLazyLoadEnable)
            {
                image.EffectiveViewportChanged -= ImageLazyLoad;
                await SetSourceAsync(image, image.Source);
                return;
            }

            // 当图像距离视窗距离小于窗口高度的 1.5 倍时，开始加载图像
            // 此处实际上应该使用 ScrollViewer 的高度，但 ScrollViewer 高度不确定，此处使用窗口高度以兼容大部分情况
            if (args.BringIntoViewDistanceY < App.CurrentWindow?.Bounds.Height * 1.5)
            {
                image.EffectiveViewportChanged -= ImageLazyLoad;
                await SetSourceAsync(image, image.Source);
            }
        }
    }

    public enum ImageStatus
    {
        Idle = 0,
        Loading,
        Success,
        Error,
    }
}
