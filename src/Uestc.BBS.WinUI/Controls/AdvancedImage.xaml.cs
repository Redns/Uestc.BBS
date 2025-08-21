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
        /// �ļ��������
        /// </summary>
        private static readonly IFileCache _fileCache =
            ServiceExtension.Services.GetRequiredService<IFileCache>();

        /// <summary>
        /// ��־����
        /// </summary>
        private static readonly ILogService _logService =
            ServiceExtension.Services.GetRequiredService<ILogService>();

        /// <summary>
        /// ͼƬԴ
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
        /// ����״̬
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
        /// �Ƿ����û���
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
        /// ͼƬ״̬
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
        /// ͼ��������
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

            // ��ͼ������Ӵ��ײ�С���������ڸ߶�ʱ����ʼ����ͼ��
            // �˴�ʵ����Ӧ��ʹ�� ScrollViewer �ĸ߶ȣ��� ScrollViewer �߶Ȳ�ȷ����
            // ������ҳ��ʾ�����б�� ScrollViewer �߶��봰�ڽ��ƣ���ʹ�� App.CurrentWindow.Bounds.Height
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
