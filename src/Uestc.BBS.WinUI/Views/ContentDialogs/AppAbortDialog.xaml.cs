using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Uestc.BBS.WinUI.Views.ContentDialogs
{
    public sealed partial class AppAbortDialog : UserControl
    {
        /// <summary>
        /// 异常
        /// </summary>
        private static readonly DependencyProperty ExceptionProperty = DependencyProperty.Register(
            nameof(Exception),
            typeof(Exception),
            typeof(AppAbortDialog),
            new PropertyMetadata(null)
        );

        public Exception Exception
        {
            get => (Exception)GetValue(ExceptionProperty);
            set => SetValue(ExceptionProperty, value);
        }

        /// <summary>
        /// 是否重启应用
        /// </summary>
        private static readonly DependencyProperty RestartAppProperty = DependencyProperty.Register(
            nameof(RestartApp),
            typeof(bool),
            typeof(AppAbortDialog),
            new PropertyMetadata(true)
        );

        public bool RestartApp
        {
            get => (bool)GetValue(RestartAppProperty);
            set => SetValue(RestartAppProperty, value);
        }

        /// <summary>
        /// 是否反馈
        /// </summary>
        private static readonly DependencyProperty FeedbackProperty = DependencyProperty.Register(
            nameof(Feedback),
            typeof(bool),
            typeof(AppAbortDialog),
            new PropertyMetadata(true)
        );

        public bool Feedback
        {
            get => (bool)GetValue(FeedbackProperty);
            set => SetValue(FeedbackProperty, value);
        }

        /// <summary>
        /// 反馈内容
        /// </summary>
        private static readonly DependencyProperty FeekbackContentProperty =
            DependencyProperty.Register(
                nameof(FeekbackContent),
                typeof(string),
                typeof(AppAbortDialog),
                new PropertyMetadata(null)
            );

        public string FeekbackContent
        {
            get => (string)GetValue(FeekbackContentProperty);
            set => SetValue(FeekbackContentProperty, value);
        }

        public AppAbortDialog()
        {
            InitializeComponent();
        }
    }
}
