using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core.Models;

namespace Uestc.BBS.Mvvm.Models
{
    public class StorageSettingModel(StorageSetting setting) : ObservableObject
    {
        public SyncSettingModel Sync { get; init; } = new(setting.Sync);
    }

    /// <summary>
    /// 同步设置
    /// </summary>
    public class SyncSettingModel(SyncSetting setting) : ObservableObject
    {
        /// <summary>
        /// 同步模式
        /// </summary>
        public SyncMode Mode
        {
            get => setting.Mode;
            set =>
                SetProperty(setting.Mode, value, setting, (setting, mode) => setting.Mode = mode);
        }

        /// <summary>
        /// 同步密钥
        /// </summary>
        public string Secret
        {
            get => setting.Secret;
            set =>
                SetProperty(
                    setting.Secret,
                    value,
                    setting,
                    (setting, secret) => setting.Secret = secret
                );
        }

        /// <summary>
        /// 最近更新日期
        /// 该日期用于判断是否同步云端设置至本地，在某些情况下用户设备 ID 更改导致本地设置丢失，如果
        /// 该属性默认值为 DateTime.Now，则默认设置将覆盖云端设置
        /// </summary>
        public DateTime LastUpdateTime
        {
            get => setting.LastUpdateTime;
            set =>
                SetProperty(
                    setting.LastUpdateTime,
                    value,
                    setting,
                    (setting, lastUpdateTime) => setting.LastUpdateTime = lastUpdateTime
                );
        }

        /// <summary>
        /// 更新时间间隔
        /// </summary>
        public TimeSpan TimeInterval
        {
            get => setting.TimeInterval;
            set =>
                SetProperty(
                    setting.TimeInterval,
                    value,
                    setting,
                    (setting, timeInterval) => setting.TimeInterval = timeInterval
                );
        }

        /// <summary>
        /// WebDAV 服务地址
        /// </summary>
        public string Api
        {
            get => setting.Api;
            set => SetProperty(setting.Api, value, setting, (setting, api) => setting.Api = api);
        }

        /// <summary>
        /// WebDAV 服务用户名
        /// </summary>
        public string Username
        {
            get => setting.Username;
            set =>
                SetProperty(
                    setting.Username,
                    value,
                    setting,
                    (setting, username) => setting.Username = username
                );
        }

        /// <summary>
        /// WebDAV 服务密码
        /// </summary>
        public string Password
        {
            get => setting.Password;
            set =>
                SetProperty(
                    setting.Password,
                    value,
                    setting,
                    (setting, password) => setting.Password = password
                );
        }
    }
}
