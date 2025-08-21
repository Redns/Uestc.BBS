using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.FileCache;
using Uestc.BBS.Core.Services.System;

namespace Uestc.BBS.WinUI.Controls
{
    public sealed partial class AdvancedImage : UserControl
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
            new PropertyMetadata(null)
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
        /// 是否启用缓存
        /// </summary>
        public static readonly DependencyProperty IsCachedEnableProperty =
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

        public AdvancedImage()
        {
            InitializeComponent();
            EffectiveViewportChanged += ImageLazyLoad;
        }

        private static async Task SetSourceAsync(AdvancedImage image, string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return;
            }

            image.Status = ImageStatus.Loading;

            try
            {
                var imageUri = image.IsCachedEnable
                    ? await _fileCache.GetFileUriAsync(new Uri(source))
                    : new Uri(source);
                image.SuccessImage.Source = new BitmapImage(imageUri)
                {
                    DecodePixelHeight =
                        image.Height is not double.PositiveInfinity ? (int)image.Height
                        : image.MaxHeight is not double.PositiveInfinity ? (int)image.MaxHeight
                        : 0,
                };

                image.Status = ImageStatus.Success;
            }
            catch (TaskCanceledException)
            {
                image.Status = ImageStatus.Timeout;
            }
            catch (Exception ex)
            {
                image.Status = ImageStatus.Error;

                _logService.Error($"Image source ({source}) is invalid", ex);
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

            // 当图像距离视窗底部小于两倍窗口高度时，开始加载图像
            // 此处实际上应该使用 ScrollViewer 的高度，但 ScrollViewer 高度不确定，
            // 由于首页显示主题列表的 ScrollViewer 高度与窗口近似，故使用 App.CurrentWindow.Bounds.Height
            if (args.BringIntoViewDistanceY < App.CurrentWindow?.Bounds.Height)
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
        Timeout,
        Error,
    }
}
