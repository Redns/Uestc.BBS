using System;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
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

        private void OpenPreviewImage(object sender, PointerRoutedEventArgs e)
        {
            var window = new WindowEx
            {
                Content = new ScrollViewer
                {
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                    ZoomMode = ZoomMode.Enabled,
                    Content = new Image()
                    {
                        Source = PreviewSource,
                        Stretch = Stretch.Uniform,
                        MaxWidth = 1100,
                        MaxHeight = 700,
                    },
                },
                ExtendsContentIntoTitleBar = true,
                SystemBackdrop = new MicaBackdrop(),
                PresenterKind = AppWindowPresenterKind.CompactOverlay,
            };

            window.SetWindowSize(1200, 800);
            window.Activate();
            window.CenterOnScreen();
        }
    }
}
