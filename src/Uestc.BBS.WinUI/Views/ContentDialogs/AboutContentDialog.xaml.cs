using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Uestc.BBS.WinUI.Views.ContentDialogs
{
    public sealed partial class AboutContentDialog : Page
    {
        /// <summary>
        /// �汾��
        /// </summary>
        private static readonly DependencyProperty VersionProperty = DependencyProperty.Register(
            nameof(Version),
            typeof(string),
            typeof(AboutContentDialog),
            new PropertyMetadata(default(string))
        );

        public string Version
        {
            get => (string)GetValue(VersionProperty);
            set => SetValue(VersionProperty, value);
        }

        /// <summary>
        /// ��Ȩ��Ϣ
        /// </summary>
        private static readonly DependencyProperty CopyRightProperty = DependencyProperty.Register(
            nameof(CopyRight),
            typeof(string),
            typeof(AboutContentDialog),
            new PropertyMetadata(default(string))
        );

        public string CopyRight
        {
            get => (string)GetValue(CopyRightProperty);
            set => SetValue(CopyRightProperty, value);
        }

        /// <summary>
        /// Դ��ֿ��ַ
        /// </summary>
        private static readonly DependencyProperty SourceRepositoryUrlProperty =
            DependencyProperty.Register(
                nameof(SourceRepositoryUrl),
                typeof(string),
                typeof(AboutContentDialog),
                new PropertyMetadata(default(string))
            );

        public string SourceRepositoryUrl
        {
            get => (string)GetValue(SourceRepositoryUrlProperty);
            set => SetValue(SourceRepositoryUrlProperty, value);
        }

        /// <summary>
        /// ������ַ
        /// </summary>
        private static readonly DependencyProperty FeedbackUrlProperty =
            DependencyProperty.Register(
                nameof(FeedbackUrl),
                typeof(string),
                typeof(AboutContentDialog),
                new PropertyMetadata(default(string))
            );

        public string FeedbackUrl
        {
            get => (string)GetValue(FeedbackUrlProperty);
            set => SetValue(FeedbackUrlProperty, value);
        }

        public AboutContentDialog()
        {
            InitializeComponent();
        }
    }
}
