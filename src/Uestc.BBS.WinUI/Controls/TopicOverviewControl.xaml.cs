using System;
using System.Linq;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Uestc.BBS.WinUI.Helpers;
using WinUIEx;

namespace Uestc.BBS.WinUI.Controls
{
    public sealed partial class TopicOverviewControl : UserControl
    {
        /// <summary>
        /// 头像
        /// </summary>
        private static readonly DependencyProperty AvatarProperty = DependencyProperty.Register(
            nameof(Avatar),
            typeof(ImageSource),
            typeof(TopicOverviewControl),
            new PropertyMetadata(default(ImageSource))
        );

        public ImageSource Avatar
        {
            get => (ImageSource)GetValue(AvatarProperty);
            set => SetValue(AvatarProperty, value);
        }

        /// <summary>
        /// 用户名
        /// </summary>
        private static readonly DependencyProperty UsernameProperty = DependencyProperty.Register(
            nameof(Username),
            typeof(string),
            typeof(TopicOverviewControl),
            new PropertyMetadata(default(string))
        );

        public string Username
        {
            get => (string)GetValue(UsernameProperty);
            set => SetValue(UsernameProperty, value);
        }

        /// <summary>
        /// 最新日期
        /// 热门帖子为发表时间，其余帖子为最新回复时间
        /// </summary>
        private static readonly DependencyProperty DateProperty = DependencyProperty.Register(
            nameof(Date),
            typeof(DateTime),
            typeof(TopicOverviewControl),
            new PropertyMetadata(default(DateTime))
        );

        public DateTime Date
        {
            get => (DateTime)GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }

        /// <summary>
        /// 是否热门
        /// </summary>
        private static readonly DependencyProperty IsHotProperty = DependencyProperty.Register(
            nameof(IsHot),
            typeof(bool),
            typeof(TopicOverviewControl),
            new PropertyMetadata(default(bool))
        );

        public bool IsHot
        {
            get => (bool)GetValue(IsHotProperty);
            set => SetValue(IsHotProperty, value);
        }

        /// <summary>
        /// 版块名称
        /// </summary>
        private static readonly DependencyProperty BoardProperty = DependencyProperty.Register(
            nameof(BoardName),
            typeof(string),
            typeof(TopicOverviewControl),
            new PropertyMetadata(default(string))
        );

        public string BoardName
        {
            get => (string)GetValue(BoardProperty);
            set => SetValue(BoardProperty, value);
        }

        /// <summary>
        /// 标题
        /// </summary>
        private static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(TopicOverviewControl),
            new PropertyMetadata(default(string))
        );

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        /// <summary>
        /// 摘要
        /// </summary>
        private static readonly DependencyProperty SubjectProperty = DependencyProperty.Register(
            nameof(Subject),
            typeof(string),
            typeof(TopicOverviewControl),
            new PropertyMetadata(default(string))
        );

        public string Subject
        {
            get => (string)GetValue(SubjectProperty);
            set => SetValue(SubjectProperty, value);
        }

        private static readonly DependencyProperty SummaryProperty = DependencyProperty.Register(
            nameof(Summary),
            typeof(string),
            typeof(TopicOverviewControl),
            new PropertyMetadata(default(string))
        );

        public string Summary
        {
            get => (string)GetValue(SummaryProperty);
            set => SetValue(SummaryProperty, value);
        }

        /// <summary>
        /// 预览图片地址
        /// </summary>
        private static readonly DependencyProperty PreviewSourceProperty =
            DependencyProperty.Register(
                nameof(PreviewSource),
                typeof(ImageSource),
                typeof(TopicOverviewControl),
                new PropertyMetadata(default(ImageSource))
            );

        public ImageSource PreviewSource
        {
            get => (ImageSource)GetValue(PreviewSourceProperty);
            set => SetValue(PreviewSourceProperty, value);
        }

        private static readonly DependencyProperty PreviewSourcesProperty =
            DependencyProperty.Register(
                nameof(PreviewSources),
                typeof(string[]),
                typeof(TopicOverviewControl),
                new PropertyMetadata(default(string[]))
            );

        public string[] PreviewSources
        {
            get => (string[])GetValue(PreviewSourcesProperty);
            set
            {
                SetValue(PreviewSourcesProperty, value);
                SetPreviewSourcesGrid();
            }
        }

        /// <summary>
        /// 浏览量
        /// </summary>
        private static readonly DependencyProperty ViewsProperty = DependencyProperty.Register(
            nameof(Views),
            typeof(uint),
            typeof(TopicOverviewControl),
            new PropertyMetadata(default(uint))
        );

        public uint Views
        {
            get => (uint)GetValue(ViewsProperty);
            set => SetValue(ViewsProperty, value);
        }

        /// <summary>
        /// 回复数
        /// </summary>
        private static readonly DependencyProperty RepliesProperty = DependencyProperty.Register(
            nameof(Replies),
            typeof(uint),
            typeof(TopicOverviewControl),
            new PropertyMetadata(default(uint))
        );

        public uint Replies
        {
            get => (uint)GetValue(RepliesProperty);
            set => SetValue(RepliesProperty, value);
        }

        /// <summary>
        /// 点赞数
        /// </summary>
        private static readonly DependencyProperty LikesProperty = DependencyProperty.Register(
            nameof(Likes),
            typeof(uint),
            typeof(TopicOverviewControl),
            new PropertyMetadata(default(uint))
        );

        public uint Likes
        {
            get => (uint)GetValue(LikesProperty);
            set => SetValue(LikesProperty, value);
        }

        public TopicOverviewControl()
        {
            InitializeComponent();
        }

        private void SetPreviewSourcesGrid()
        {
            if (PreviewSources.Length == 0)
            {
                return;
            }

            var images = PreviewSources
                .Select(s =>
                {
                    var image = new Image
                    {
                        Source = new BitmapImage(new Uri(s)),
                        Stretch = Stretch.UniformToFill,
                    };
                    image.PointerPressed += OpenPreviewImage;
                    return image;
                })
                .ToArray();
            var borderImages = images
                .Select(i => new Border
                {
                    CornerRadius = new CornerRadius(6),
                    Child = i,
                    HorizontalAlignment = HorizontalAlignment.Left,
                })
                .ToArray();

            if (PreviewSources.Length == 1)
            {
                images[0].Stretch = Stretch.Uniform;
                PreviewSourcesGrid.Add(borderImages[0]);
                return;
            }

            if (PreviewSources.Length == 2)
            {
                PreviewSourcesGrid.ColumnSpacing = 10;
                PreviewSourcesGrid.SetColumnDefinitions("1*,1*");

                images[0].Height = 150;
                images[1].Height = 150;

                PreviewSourcesGrid.Add(borderImages[0]);
                PreviewSourcesGrid.Add(borderImages[1], 0, 1);
                return;
            }

            PreviewSourcesGrid.RowSpacing = 10;
            PreviewSourcesGrid.ColumnSpacing = 10;

            if (PreviewSources.Length == 3)
            {
                PreviewSourcesGrid.SetRowDefinitions("1*,1*");
                PreviewSourcesGrid.SetColumnDefinitions("1*,1*");

                PreviewSourcesGrid.Add(borderImages[0], 0, 0, rowSpan: 2);
                PreviewSourcesGrid.Add(borderImages[1], 0, 1);
                PreviewSourcesGrid.Add(borderImages[2], 1, 1);
                return;
            }

            if (PreviewSources.Length == 4)
            {
                PreviewSourcesGrid.SetRowDefinitions("1*,1*");
                PreviewSourcesGrid.SetColumnDefinitions("1*,1*");

                PreviewSourcesGrid.Add(borderImages[0]);
                PreviewSourcesGrid.Add(borderImages[1], 0, 1);
                PreviewSourcesGrid.Add(borderImages[2], 1, 0);
                PreviewSourcesGrid.Add(borderImages[3], 1, 1);
                return;
            }

            if (PreviewSources.Length == 5)
            {
                PreviewSourcesGrid.SetRowDefinitions("1*,1*");
                PreviewSourcesGrid.SetColumnDefinitions("1*,1*,1*");

                PreviewSourcesGrid.Add(borderImages[0], rowSpan: 2);
                PreviewSourcesGrid.Add(borderImages[1], 0, 1);
                PreviewSourcesGrid.Add(borderImages[2], 0, 2);
                PreviewSourcesGrid.Add(borderImages[3], 1, 1);
                PreviewSourcesGrid.Add(borderImages[4], 1, 2);
                return;
            }

            if (PreviewSources.Length == 6)
            {
                PreviewSourcesGrid.SetRowDefinitions("1*,1*");
                PreviewSourcesGrid.SetColumnDefinitions("1*,1*,1*");

                PreviewSourcesGrid.Add(borderImages[0], 0, 0);
                PreviewSourcesGrid.Add(borderImages[1], 0, 1);
                PreviewSourcesGrid.Add(borderImages[2], 0, 2);
                PreviewSourcesGrid.Add(borderImages[3], 1, 0);
                PreviewSourcesGrid.Add(borderImages[4], 1, 1);
                PreviewSourcesGrid.Add(borderImages[5], 1, 2);
                return;
            }
        }

        /// <summary>
        /// 打开新窗口预览图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenPreviewImage(object sender, PointerRoutedEventArgs e)
        {
            if (sender is not Image image)
            {
                return;
            }

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
                        ItemsSource = PreviewSources,
                        ItemTemplate = PreviewFlipViewDataTemplete,
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
