using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core.Models;

namespace Uestc.BBS.Mvvm.Models
{
    public class NotificationSettingModel(NotificationSetting setting) : ObservableObject
    {
        /// <summary>
        /// 是否开启通知
        /// </summary>
        public bool IsEnabled
        {
            get => setting.IsEnabled;
            set => SetProperty(setting.IsEnabled, value, setting, (s, e) => s.IsEnabled = e);
        }

        /// <summary>
        /// 系统
        /// </summary>
        public bool IsSystenMessageEnabled
        {
            get => setting.IsSystenMessageEnabled;
            set =>
                SetProperty(
                    setting.IsSystenMessageEnabled,
                    value,
                    setting,
                    (s, e) => s.IsSystenMessageEnabled = e
                );
        }

        /// <summary>
        /// 私信
        /// </summary>
        public bool IsPrivateMessageEnabled
        {
            get => setting.IsPrivateMessageEnabled;
            set =>
                SetProperty(
                    setting.IsPrivateMessageEnabled,
                    value,
                    setting,
                    (s, e) => s.IsPrivateMessageEnabled = e
                );
        }

        /// <summary>
        /// 回复
        /// </summary>
        public bool IsNewReplyEnabled
        {
            get => setting.IsNewReplyEnabled;
            set =>
                SetProperty(
                    setting.IsNewReplyEnabled,
                    value,
                    setting,
                    (s, e) => s.IsNewReplyEnabled = e
                );
        }

        /// <summary>
        /// @我
        /// </summary>
        public bool IsAtMeEnabled
        {
            get => setting.IsAtMeEnabled;
            set =>
                SetProperty(setting.IsAtMeEnabled, value, setting, (s, e) => s.IsAtMeEnabled = e);
        }

        /// <summary>
        /// 点评
        /// </summary>
        public bool IsReviewEnabled
        {
            get => setting.IsReviewEnabled;
            set =>
                SetProperty(
                    setting.IsReviewEnabled,
                    value,
                    setting,
                    (s, e) => s.IsReviewEnabled = e
                );
        }

        /// <summary>
        /// 插眼
        /// </summary>
        public bool IsMarkEnabled
        {
            get => setting.IsMarkEnabled;
            set =>
                SetProperty(setting.IsMarkEnabled, value, setting, (s, e) => s.IsMarkEnabled = e);
        }
    }
}
