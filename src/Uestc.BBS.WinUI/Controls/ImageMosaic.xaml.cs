using System;
using System.Linq;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Uestc.BBS.WinUI.Helpers;
using WinUIEx;

namespace Uestc.BBS.WinUI.Controls
{
    public sealed partial class ImageMosaic : UserControl
    {
        /// <summary>
        /// 预览图片数据源
        /// </summary>
        private static readonly DependencyProperty PreviewSourcesProperty =
            DependencyProperty.Register(
                nameof(PreviewSources),
                typeof(string[]),
                typeof(ImageMosaic),
                new PropertyMetadata(null, SetPreviewGrid)
            );

        public string[] PreviewSources
        {
            get => (string[])GetValue(PreviewSourcesProperty);
            set => SetValue(PreviewSourcesProperty, value);
        }

        /// <summary>
        /// 是否对预览图片进行优化，默认关闭
        /// </summary>
        private static readonly DependencyProperty PreviewImageDecodeOptimizedProperty =
            DependencyProperty.Register(
                nameof(PreviewImageDecodeOptimized),
                typeof(bool),
                typeof(ImageMosaic),
                new PropertyMetadata(false)
            );

        public bool PreviewImageDecodeOptimized
        {
            get => (bool)GetValue(PreviewImageDecodeOptimizedProperty);
            set => SetValue(PreviewImageDecodeOptimizedProperty, value);
        }

        public ImageMosaic()
        {
            InitializeComponent();
        }

        private static void SetPreviewGrid(
            DependencyObject sender,
            DependencyPropertyChangedEventArgs e
        )
        {
            if (e.NewValue is not string[] sources || sources.Length == 0)
            {
                return;
            }

            if (sender is not ImageMosaic imageMosaic)
            {
                return;
            }

            if (sources.Length == 1)
            {
                var image = new Image
                {
                    MaxHeight = 240,
                    Stretch = Stretch.Uniform,
                    Source = new BitmapImage(new Uri(sources[0]))
                    {
                        DecodePixelHeight = imageMosaic.PreviewImageDecodeOptimized ? 240 : 0,
                    },
                };
                image.PointerPressed += (_, _) =>
                    OpenPreviewImage(
                        imageMosaic.PreviewSources,
                        0,
                        imageMosaic.PreviewFlipViewDataTemplete
                    );

                imageMosaic.Content = new Border
                {
                    Child = image,
                    CornerRadius = new CornerRadius(6),
                    HorizontalAlignment = HorizontalAlignment.Left,
                };
                return;
            }

            var grid = new Grid() { RowSpacing = 5, ColumnSpacing = 5 };
            imageMosaic.Content = grid;

            // 计算 Grid 布局
            var colums = sources.Length switch
            {
                <= 4 => 2,
                _ => 3,
            };
            var rows = (int)Math.Ceiling(sources.Length / (double)colums);
            var imageHeight = 130 - rows * 15;
            grid.SetRowsAndColumns(rows, colums);

            var images = sources
                .Take(9)
                .Select(
                    (s, index) =>
                    {
                        var image = new Image
                        {
                            // 图像较多时限制其整体高度，避免占据太多视觉空间
                            Height =
                                (index is 0 && sources.Length is 3)
                                    ? imageHeight * 2 + grid.RowSpacing
                                    : imageHeight,
                            Source = new BitmapImage(new Uri(s))
                            {
                                DecodePixelHeight = imageMosaic.PreviewImageDecodeOptimized
                                    ? 140 - rows * 15
                                    : 0,
                            },
                            Stretch = Stretch.UniformToFill,
                        };
                        image.PointerPressed += (_, e) =>
                            OpenPreviewImage(
                                imageMosaic.PreviewSources,
                                Array.IndexOf(sources, s),
                                imageMosaic.PreviewFlipViewDataTemplete
                            );
                        return image;
                    }
                )
                .Select(image => new Border
                {
                    Child = image,
                    CornerRadius = new CornerRadius(6),
                    HorizontalAlignment = HorizontalAlignment.Left,
                })
                .ToArray();

            if (sources.Length == 3)
            {
                grid.Add(images[0], 0, 0, 2, 1);
                grid.Add(images[1], 0, 1);
                grid.Add(images[2], 1, 1);
                return;
            }

            for (var i = 0; i < images.Length; i++)
            {
                grid.Add(images[i], i / colums, i % colums);
            }
        }

        /// <summary>
        /// 打开新窗口预览图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OpenPreviewImage(
            string[] sources,
            int currentSourceIndex,
            DataTemplate dataTemplate
        )
        {
            var window = new WindowEx
            {
                ExtendsContentIntoTitleBar = true,
                SystemBackdrop = new MicaBackdrop(),
                PresenterKind = AppWindowPresenterKind.CompactOverlay,
                Content = new ScrollViewer
                {
                    ZoomMode = ZoomMode.Enabled,
                    Content = new FlipView
                    {
                        ItemsSource = sources,
                        ItemTemplate = dataTemplate,
                        SelectedIndex = currentSourceIndex,
                        Background = new SolidColorBrush(Colors.Transparent),
                    },
                    VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
                },
            };

            window.SetWindowSize(1200, 800);
            window.Activate();
            window.CenterOnScreen();
        }
    }
}
