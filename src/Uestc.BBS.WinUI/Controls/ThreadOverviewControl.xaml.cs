using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Uestc.BBS.WinUI.Controls
{
    public sealed partial class ThreadOverviewControl : UserControl
    {
        /// <summary>
        /// ͷ��
        /// </summary>
        private static readonly DependencyProperty AvatarProperty = DependencyProperty.Register(
            nameof(Avatar),
            typeof(string),
            typeof(ThreadOverviewControl),
            new PropertyMetadata(default(string))
        );

        public string Avatar
        {
            get => (string)GetValue(AvatarProperty);
            set => SetValue(AvatarProperty, value);
        }

        /// <summary>
        /// �û���
        /// </summary>
        private static readonly DependencyProperty UsernameProperty = DependencyProperty.Register(
            nameof(Username),
            typeof(string),
            typeof(ThreadOverviewControl),
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
            typeof(ThreadOverviewControl),
            new PropertyMetadata(default(DateTime))
        );

        public DateTime Date
        {
            get => (DateTime)GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }

        /// <summary>
        /// �Ƿ�Ϊ��������
        /// </summary>
        private static readonly DependencyProperty IsHotProperty = DependencyProperty.Register(
            nameof(IsHot),
            typeof(bool),
            typeof(ThreadOverviewControl),
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
        private static readonly DependencyProperty BoardNameProperty = DependencyProperty.Register(
            nameof(BoardName),
            typeof(string),
            typeof(ThreadOverviewControl),
            new PropertyMetadata(default(string))
        );

        public string BoardName
        {
            get => (string)GetValue(BoardNameProperty);
            set => SetValue(BoardNameProperty, value);
        }

        /// <summary>
        /// ����
        /// </summary>
        private static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(ThreadOverviewControl),
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
            typeof(ThreadOverviewControl),
            new PropertyMetadata(default(string))
        );

        public string Subject
        {
            get => (string)GetValue(SubjectProperty);
            set => SetValue(SubjectProperty, value);
        }

        /// <summary>
        /// Ԥ��ͼƬ��ַ
        /// </summary>
        private static readonly DependencyProperty PreviewSourcesProperty =
            DependencyProperty.Register(
                nameof(PreviewSources),
                typeof(string[]),
                typeof(ThreadOverviewControl),
                new PropertyMetadata(default(string[]))
            );

        public string[] PreviewSources
        {
            get => (string[])GetValue(PreviewSourcesProperty);
            set => SetValue(PreviewSourcesProperty, value);
        }

        /// <summary>
        /// �Ƿ���ʾԤ��ͼƬ
        /// </summary>
        private static readonly DependencyProperty ShowPreviewImageProperty =
            DependencyProperty.Register(
                nameof(ShowPreviewImage),
                typeof(bool),
                typeof(ThreadOverviewControl),
                new PropertyMetadata(default(bool))
            );

        public bool ShowPreviewImage
        {
            get => (bool)GetValue(ShowPreviewImageProperty);
            set => SetValue(ShowPreviewImageProperty, value);
        }

        /// <summary>
        /// �����
        /// </summary>
        private static readonly DependencyProperty ViewsProperty = DependencyProperty.Register(
            nameof(Views),
            typeof(uint),
            typeof(ThreadOverviewControl),
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
            typeof(ThreadOverviewControl),
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
            typeof(ThreadOverviewControl),
            new PropertyMetadata(default(uint))
        );

        public uint Likes
        {
            get => (uint)GetValue(LikesProperty);
            set => SetValue(LikesProperty, value);
        }

        public ThreadOverviewControl()
        {
            InitializeComponent();
        }
    }
}
