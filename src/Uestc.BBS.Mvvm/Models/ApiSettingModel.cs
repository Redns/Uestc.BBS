using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core.Models;

namespace Uestc.BBS.Mvvm.Models
{
    public class ApiSettingModel(ApiSetting setting) : ObservableObject
    {
        public bool IsSystermProxyEnabled
        {
            get => setting.IsSystemProxyEnabled;
            set =>
                SetProperty(
                    setting.IsSystemProxyEnabled,
                    value,
                    setting,
                    (s, e) => s.IsSystemProxyEnabled = e
                );
        }

        /// <summary>
        /// 是否启用证书验证
        /// </summary>
        public bool IsCertificateVerificationEnabled
        {
            get => setting.IsCertificateVerificationEnabled;
            set =>
                SetProperty(
                    setting.IsCertificateVerificationEnabled,
                    value,
                    setting,
                    (s, e) => s.IsCertificateVerificationEnabled = e
                );
        }

        /// <summary>
        /// 基地址
        /// </summary>
        public string BaseUrl
        {
            get => setting.BaseUrl;
            set => SetProperty(setting.BaseUrl, value, setting, (s, e) => s.BaseUrl = e);
        }

        /// <summary>
        /// 每日一句
        /// </summary>
        public string DailySentenceUrl
        {
            get => setting.DailySentenceUrl;
            set =>
                SetProperty(
                    setting.DailySentenceUrl,
                    value,
                    setting,
                    (s, e) => s.DailySentenceUrl = e
                );
        }

        /// <summary>
        /// 授权
        /// </summary>
        public string AuthUrl
        {
            get => setting.AuthUrl;
            set => SetProperty(setting.AuthUrl, value, setting, (s, e) => s.AuthUrl = e);
        }

        /// <summary>
        /// 主题详情
        /// </summary>
        public string TopicDetailUrl
        {
            get => setting.TopicDetailUrl;
            set =>
                SetProperty(setting.TopicDetailUrl, value, setting, (s, e) => s.TopicDetailUrl = e);
        }

        /// <summary>
        /// 主题列表
        /// </summary>
        public string TopicListUrl
        {
            get => setting.TopicListUrl;
            set => SetProperty(setting.TopicListUrl, value, setting, (s, e) => s.TopicListUrl = e);
        }

        /// <summary>
        /// 用户详情
        /// </summary>
        public string UserDetailUrl
        {
            get => setting.UserDetailUrl;
            set =>
                SetProperty(setting.UserDetailUrl, value, setting, (s, e) => s.UserDetailUrl = e);
        }
    }
}
