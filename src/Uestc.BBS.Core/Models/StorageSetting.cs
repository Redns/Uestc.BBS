using FastEnumUtility;

namespace Uestc.BBS.Core.Models
{
    public class StorageSetting
    {
        public CacheSetting Cache { get; set; } = new();

        public SyncSetting Sync { get; set; } = new();
    }

    /// <summary>
    /// 缓存设置
    /// </summary>
    public class CacheSetting
    {
        /// <summary>
        /// 是否启用缓存
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 缓存根目录
        /// </summary>
        public string RootDirectory { get; set; } =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                AppDomain.CurrentDomain.FriendlyName,
                "Cache"
            );
    }

    /// <summary>
    /// 同步设置
    /// </summary>
    public class SyncSetting
    {
        /// <summary>
        /// 同步模式
        /// </summary>
        public SyncMode Mode { get; set; } = SyncMode.OnStaupAndTiming;

        /// <summary>
        /// 同步密钥
        /// </summary>
        public string Secret { get; set; } = string.Empty;

        /// <summary>
        /// 最近更新日期
        /// 该日期用于判断是否同步云端设置至本地，在某些情况下用户设备 ID 更改导致本地设置丢失，如果
        /// 该属性默认值为 DateTime.Now，则默认设置将覆盖云端设置
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 更新时间间隔
        /// </summary>
        public TimeSpan TimeInterval { get; set; } = TimeSpan.FromMinutes(60);

        /// <summary>
        /// WebDAV 服务地址
        /// </summary>
        public string Api { get; set; } = "https://dav.jianguoyun.com/dav";

        /// <summary>
        /// WebDAV 服务用户名
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// WebDAV 服务密码
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// 同步模式
    /// </summary>
    public enum SyncMode
    {
        [Label("手动")]
        None = 0,

        [Label("启动时同步")]
        OnStartup,

        [Label("启动时+定时同步")]
        OnStaupAndTiming,
    }
}
