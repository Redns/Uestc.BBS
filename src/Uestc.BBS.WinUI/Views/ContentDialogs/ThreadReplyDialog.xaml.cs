using System;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Sdk;
using Uestc.BBS.Sdk.Services.Thread.ThreadReply;

namespace Uestc.BBS.WinUI.Views.ContentDialogs
{
    public sealed partial class ThreadReplyDialog : UserControl
    {
        private readonly ILogService _logService =
            ServiceExtension.Services.GetRequiredService<ILogService>();

        private readonly IThreadReplyService _threadReplyService =
            ServiceExtension.Services.GetRequiredKeyedService<IThreadReplyService>(
                ServiceExtensions.WEB_API
            );

        private readonly INotificationService _notificationService =
            ServiceExtension.Services.GetRequiredService<INotificationService>();

        /// <summary>
        /// ��ǰ�ظ������� ID
        /// </summary>
        private static readonly DependencyProperty ThreadIdProperty = DependencyProperty.Register(
            nameof(ThreadId),
            typeof(uint),
            typeof(ThreadReplyDialog),
            new PropertyMetadata(0)
        );

        public uint ThreadId
        {
            get => (uint)GetValue(ThreadIdProperty);
            set => SetValue(ThreadIdProperty, value);
        }

        /// <summary>
        /// ��ǰ�û�ͷ��
        /// </summary>
        private static readonly DependencyProperty UserAvatarProperty = DependencyProperty.Register(
            nameof(UserAvatar),
            typeof(string),
            typeof(ThreadReplyDialog),
            new PropertyMetadata(string.Empty)
        );

        public string UserAvatar
        {
            get => (string)GetValue(UserAvatarProperty);
            set => SetValue(UserAvatarProperty, value);
        }

        /// <summary>
        /// �ظ�����
        /// </summary>
        private static readonly DependencyProperty ReplyContentProperty =
            DependencyProperty.Register(
                nameof(ReplyContent),
                typeof(string),
                typeof(ThreadReplyDialog),
                new PropertyMetadata(string.Empty)
            );

        public string ReplyContent
        {
            get => (string)GetValue(ReplyContentProperty);
            set => SetValue(ReplyContentProperty, value);
        }

        #region ���õ�����
        /// <summary>
        /// ��ǰ�ظ������۵� ID
        /// </summary>
        private static readonly DependencyProperty QuoteIdProperty = DependencyProperty.Register(
            nameof(QuoteId),
            typeof(uint),
            typeof(ThreadReplyDialog),
            new PropertyMetadata(0)
        );

        public uint QuoteId
        {
            get => (uint)GetValue(QuoteIdProperty);
            set => SetValue(QuoteIdProperty, value);
        }

        /// <summary>
        /// ��ǰ�ظ������۵��û�ͷ��
        /// </summary>
        private static readonly DependencyProperty QuoteUserAvatarProperty =
            DependencyProperty.Register(
                nameof(QuoteUserAvatar),
                typeof(string),
                typeof(ThreadReplyDialog),
                new PropertyMetadata(string.Empty)
            );

        public string QuoteUserAvatar
        {
            get => (string)GetValue(QuoteUserAvatarProperty);
            set => SetValue(QuoteUserAvatarProperty, value);
        }

        /// <summary>
        /// ��ǰ�ظ������۵��û���
        /// </summary>
        private static readonly DependencyProperty QuoteUsernameProperty =
            DependencyProperty.Register(
                nameof(QuoteUsername),
                typeof(string),
                typeof(ThreadReplyDialog),
                new PropertyMetadata(string.Empty)
            );

        public string QuoteUsername
        {
            get => (string)GetValue(QuoteUsernameProperty);
            set => SetValue(QuoteUsernameProperty, value);
        }

        /// <summary>
        /// ��ǰ�ظ������۵Ĵ���ʱ��
        /// </summary>
        private static readonly DependencyProperty QuoteCreateTimeProperty =
            DependencyProperty.Register(
                nameof(QuoteCreateTime),
                typeof(DateTime),
                typeof(ThreadReplyDialog),
                new PropertyMetadata(DateTime.MinValue)
            );

        public DateTime QuoteCreateTime
        {
            get => (DateTime)GetValue(QuoteCreateTimeProperty);
            set => SetValue(QuoteCreateTimeProperty, value);
        }

        /// <summary>
        /// ��ǰ�ظ������۵�����
        /// </summary>
        private static readonly DependencyProperty QuoteContentProperty =
            DependencyProperty.Register(
                nameof(QuoteContent),
                typeof(string),
                typeof(ThreadReplyDialog),
                new PropertyMetadata(string.Empty)
            );

        public string QuoteContent
        {
            get => (string)GetValue(QuoteContentProperty);
            set => SetValue(QuoteContentProperty, value);
        }

        #endregion

        public ThreadReplyDialog()
        {
            InitializeComponent();
        }

        [RelayCommand]
        public void Reply() { }

        [RelayCommand]
        private void ClearReplyContent() => ReplyContent = string.Empty;
    }
}
