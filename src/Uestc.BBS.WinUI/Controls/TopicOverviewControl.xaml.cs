using System;
using System.Linq;
using CommunityToolkit.WinUI;
using CommunityToolkit.WinUI.Controls;
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
        /// ͷ��
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
        /// �û���
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
        /// ��������
        /// ��������Ϊ����ʱ�䣬��������Ϊ���»ظ�ʱ��
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
        /// �Ƿ�����
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
        /// �������
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
        /// ����
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
        /// ժҪ
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
        /// Ԥ��ͼƬ��ַ
        /// </summary>
        private static readonly DependencyProperty PreviewSourcesProperty =
            DependencyProperty.Register(
                nameof(PreviewSources),
                typeof(string[]),
                typeof(TopicOverviewControl),
                new PropertyMetadata(default(string[]), SetPreviewGrid)
            );

        public string[] PreviewSources
        {
            get => (string[])GetValue(PreviewSourcesProperty);
            set => SetValue(PreviewSourcesProperty, value);
        }

        /// <summary>
        /// ģ�����أ�����Ԥ��ͼ��Ľ���߶��Լ�����Դռ�ã����������ٶ�
        /// </summary>
        private static readonly DependencyProperty LazyLoadingProperty =
            DependencyProperty.Register(
                nameof(LazyLoading),
                typeof(bool),
                typeof(TopicOverviewControl),
                new PropertyMetadata(false)
            );

        public bool LazyLoading
        {
            get => (bool)GetValue(LazyLoadingProperty);
            set => SetValue(LazyLoadingProperty, value);
        }

        /// <summary>
        /// �����
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
        /// �ظ���
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
        /// ������
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

        private static void SetPreviewGrid(
            DependencyObject sender,
            DependencyPropertyChangedEventArgs e
        )
        {
            if (e.NewValue is not string[] sources || sources.Length == 0)
            {
                return;
            }

            if (sender is not TopicOverviewControl topicOverview)
            {
                return;
            }

            topicOverview.PreviewGrid.Children.Clear();
            topicOverview.PreviewGrid.RowDefinitions.Clear();
            topicOverview.PreviewGrid.ColumnDefinitions.Clear();

            if (sources.Length == 1)
            {
                var image = new Image
                {
                    MaxHeight = 240,
                    Stretch = Stretch.Uniform,
                    Source = new BitmapImage(new Uri(sources[0]))
                    {
                        DecodePixelHeight = topicOverview.LazyLoading ? 240 : 0
                    },
                };
                image.PointerPressed += (sender, e) =>
                    OpenPreviewImage(
                        topicOverview.PreviewSources,
                        0,
                        topicOverview.PreviewFlipViewDataTemplete
                    );

                topicOverview.PreviewGrid.Children.Add(
                    new Border
                    {
                        Child = image,
                        CornerRadius = new CornerRadius(6),
                        HorizontalAlignment = HorizontalAlignment.Left,
                    }
                );
                return;
            }

            // ���� Grid ����
            var colums = sources.Length switch
            {
                2 => 2,
                4 => 2,
                _ => 3
            };
            var rows = (int)Math.Ceiling(sources.Length / (double)colums);
            if (rows > 1)
            {
                for (var i = 0; i < rows; i++)
                {
                    topicOverview.PreviewGrid.RowDefinitions.Add(
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                    );
                }
            }
            for (var i = 0; i < colums; i++)
            {
                topicOverview.PreviewGrid.ColumnDefinitions.Add(
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                );
            }

            var images = sources
                .Take(9)
                .Select(s =>
                {
                    var image = new Image
                    {
                        // ͼ��϶�ʱ����������߶ȣ�����ռ��̫���Ӿ��ռ�
                        Height = 140 - rows * 15,
                        Source = new BitmapImage(new Uri(s))
                        {
                            DecodePixelHeight = topicOverview.LazyLoading ? 140 - rows * 15 : 0
                        },
                        Stretch = Stretch.UniformToFill,
                    };
                    image.PointerPressed += (sender, e) =>
                        OpenPreviewImage(
                            topicOverview.PreviewSources,
                            Array.IndexOf(sources, s),
                            topicOverview.PreviewFlipViewDataTemplete
                        );
                    return image;
                })
                .Select(image => new Border
                {
                    Child = image,
                    CornerRadius = new CornerRadius(6),

                    HorizontalAlignment = HorizontalAlignment.Left,
                })
                .ToArray();

            for (var i = 0; i < images.Length; i++)
            {
                topicOverview.PreviewGrid.Add(images[i], i / colums, i % colums);
            }
        }

        /// <summary>
        /// ���´���Ԥ��ͼ��
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
