using System;
using System.Linq;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Uestc.BBS.WinUI.Helpers;
using WinUIEx;

namespace Uestc.BBS.WinUI.Controls
{
    public sealed partial class ImageMosaic : UserControl
    {
        /// <summary>
        /// 图片数据源
        /// </summary>
        private static readonly DependencyProperty SourcesProperty = DependencyProperty.Register(
            nameof(Sources),
            typeof(string[]),
            typeof(ImageMosaic),
            new PropertyMetadata(null, SetGrid)
        );

        public string[] Sources
        {
            get => (string[])GetValue(SourcesProperty);
            set => SetValue(SourcesProperty, value);
        }

        public ImageMosaic()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设置图像布局
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void SetGrid(DependencyObject sender, DependencyPropertyChangedEventArgs e)
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
                var image = new AdvancedImage
                {
                    MaxHeight = 240,
                    Stretch = Stretch.Uniform,
                    Source = sources[0],
                    IsCachedEnable = true,
                };
                image.PointerPressed += (_, _) =>
                    OpenImage(imageMosaic.Sources, 0, imageMosaic.PreviewFlipViewDataTemplete);

                imageMosaic.Content = new Border
                {
                    Child = image,
                    CornerRadius = new CornerRadius(6),
                    HorizontalAlignment = HorizontalAlignment.Left,
                };
                return;
            }

            // 多张图片采用 Grid 布局
            var grid = new Grid() { RowSpacing = 5, ColumnSpacing = 5 };
            imageMosaic.Content = grid;

            // 4 张图片及以下采用 2 列布局
            // 5 张图片及以上采用 3 列布局
            var colums = sources.Length switch
            {
                <= 4 => 2,
                _ => 3,
            };
            var rows = (int)Math.Ceiling(sources.Length / (double)colums);
            var imageHeight = 130 - rows * 15;
            grid.SetRowsAndColumns(rows, colums);

            // 最多仅显示 9 张图片
            var images = sources
                .Take(9)
                .Select(
                    (s, index) =>
                    {
                        var image = new AdvancedImage
                        {
                            // 图像较多时限制其整体高度，避免占据太多视觉空间
                            Height =
                                (index is 0 && sources.Length is 3)
                                    ? imageHeight * 2 + grid.RowSpacing
                                    : imageHeight,
                            Stretch = Stretch.UniformToFill,
                            Source = s,
                            IsCachedEnable = true,
                        };

                        image.PointerPressed += (_, _) =>
                            OpenImage(
                                imageMosaic.Sources,
                                Array.IndexOf(sources, s),
                                imageMosaic.PreviewFlipViewDataTemplete
                            );

                        return image;
                    }
                )
                .Select(image => new Border { Child = image, CornerRadius = new CornerRadius(6) })
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
        /// 打开新窗口浏览图像
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="currentSourceIndex"></param>
        /// <param name="dataTemplate"></param>
        private static void OpenImage(
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
