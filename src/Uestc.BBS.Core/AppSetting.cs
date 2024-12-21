using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Uestc.BBS.Core.Services.System;

namespace Uestc.BBS.Core
{
    public class AppSetting
    {
        /// <summary>
        /// 外观设置
        /// </summary>
        public ApperanceSetting Apperance { get; set; } = new();

        /// <summary>
        /// 授权设置
        /// </summary>
        public AuthSetting Auth { get; set; } = new();

        /// <summary>
        /// 同步设置
        /// </summary>
        public SyncSetting Sync { get; set; } = new();

        /// <summary>
        /// 日志设置
        /// </summary>
        public LogSetting Log { get; set; } = new();

        /// <summary>
        /// 更新设置
        /// </summary>
        public UpgradeSetting Upgrade { get; set; } = new();

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="path">配置文件路径</param>
        /// <param name="secret">配置文件解密密钥</param>
        /// <returns></returns>
        public static AppSetting Load(string? path = null)
        {
            AppSetting? appSetting;

            try
            {
                path ??= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppDomain.CurrentDomain.FriendlyName, "appsettings.json");
                if (File.Exists(path))
                {
                    appSetting = JsonSerializer.Deserialize<AppSetting>(File.ReadAllText(path), SerializerOptions);
                    if (appSetting is not null)
                    {
                        return appSetting;
                    }
                }
            }
            catch(Exception e)
            {
                ServiceExtension.Services.GetRequiredService<ILogService>().Error(string.Format("Appsetting file {0} load failed", path), e);
            }

            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir) && !string.IsNullOrEmpty(dir))
            {
                Directory.CreateDirectory(dir);
            }
            appSetting = new AppSetting();
            appSetting.Save(path);

            return appSetting;
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="path">配置文件路径</param>
        /// <param name="secret">配置文件加密密钥</param>
        public void Save(string? path = null)
        {
            File.WriteAllText(path ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                AppDomain.CurrentDomain.FriendlyName, "appsettings.json"), ToString());
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, SerializerOptions);
        }

        public static readonly JsonSerializerOptions SerializerOptions = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            TypeInfoResolver = JsonTypeInfoResolver.Combine(AppSettingContext.Default, new DefaultJsonTypeInfoResolver())
        };
    }

    /// <summary>
    /// 外观设置
    /// </summary>
    public class ApperanceSetting
    {
        /// <summary>
        /// 主题
        /// </summary>
        public ThemeColor ThemeMode { get; set; } = ThemeColor.System;

        /// <summary>
        /// 背景透明度
        /// </summary>
        public double BackgroundOpacity { get; set; } = 1.0;

        /// <summary>
        /// 官方论坛链接
        /// </summary>
        public string OfficialWebsite { get; set; } = "https://bbs.uestc.edu.cn/new";

        /// <summary>
        /// 静默启动
        /// </summary>
        public bool SlientStart { get; set; } = false;

        /// <summary>
        /// 开机自启动
        /// </summary>
        public bool StartupOnLaunch {  get; set; } = false;

        /// <summary>
        /// 窗口关闭行为
        /// </summary>
        public WindowCloseBehavior WindowCloseBehavior { get; set; } = WindowCloseBehavior.Hide;

        /// <summary>
        /// 菜单列表
        /// </summary>
        public MenuItem[] MenuItems { get; set; } = 
        [
            new MenuItem
            {
                Key = "Home",
                Name = "主 页",
                Symbol = "Home",
                IsActive = true,
                DockTop = true
            },
            new MenuItem
            {
                Key = "Sections",
                Name = "版 块",
                Symbol = "Apps",
                IsActive = false,
                DockTop = true
            },
            new MenuItem
            {
                Key = "Services",
                Name = "服 务",
                Symbol = "Rocket",
                IsActive = false,
                DockTop = true
            },
            new MenuItem
            {
                Key = "Moments",
                Name = "动 态",
                Symbol = "Scan",
                IsActive = false,
                DockTop = true
            },
            new MenuItem
            {
                Key = "Post",
                Name = "发 布",
                Symbol = "SaveCopy",
                IsActive = false,
                DockTop = true
            },
            new MenuItem
            {
                Key = "Settings",
                Name = "设 置",
                Symbol = "Settings",
                IsActive = false,
                DockTop = false
            },
            new MenuItem
            {
                Key = "Messages",
                Name = "消 息",
                Symbol = "Mail",
                IsActive = false,
                DockTop = false
            }
        ];
    }

    /// <summary>
    /// 主题
    /// </summary>
    public enum ThemeColor
    {
        Light = 0,
        Dark,
        System
    }

    /// <summary>
    /// 窗口关闭行为
    /// </summary>
    public enum WindowCloseBehavior
    {
        Hide,
        Exit
    }

    public enum MenuItemKey
    {
        Home = 0,
        Sections,
        Services,
        Moments,
        Post,
        Messages,
        Settings
    }

    public class MenuItem
    {
        public string Key { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Symbol { get; set; } = string.Empty;

        public bool IsActive { get; set; } = false;

        public bool DockTop { get; set; } = true;
    }

    /// <summary>
    /// 授权设置
    /// </summary>
    public class AuthSetting
    {
        /// <summary>
        /// 自动登录
        /// </summary>
        public bool AutoLogin { get; set; } = false;

        /// <summary>
        /// 记住密码（取消记住密码仍然会保存密钥信息）
        /// </summary>
        public bool RememberPassword { get; set; } = false;

        /// <summary>
        /// 默认授权信息 Uid
        /// </summary>
        public uint DefaultCredentialUid {  get; set; }

        /// <summary>
        /// 默认授权信息
        /// </summary>
        [JsonIgnore]
        public AuthCredential? DefaultCredential => Credentials.FirstOrDefault(c => c.Uid == DefaultCredentialUid);

        /// <summary>
        /// 用戶是否授权
        /// </summary>
        [JsonIgnore]
        public bool IsUserAuthed => !string.IsNullOrEmpty(DefaultCredential?.Token) && !string.IsNullOrEmpty(DefaultCredential.Secret); 

        /// <summary>
        /// 授权信息列表（保存本地所有授权信息）
        /// </summary>
        public List<AuthCredential> Credentials { get; set; } = [];
    }

    /// <summary>
    /// 授权信息
    /// </summary>
    public class AuthCredential
    {
        /// <summary>
        /// 用户唯一识别码
        /// </summary>
        public uint Uid {  get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 令牌
        /// </summary>

        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// 密钥
        /// </summary>

        public string Secret { get; set; } = string.Empty;

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; } = string.Empty;

        /// <summary>
        /// 此处序列化用户 AutoCompleteBox 显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
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
        public string Username {  get; set; } = string.Empty;

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
        None = 0,           // 手动
        OnStartup,          // 启动时同步
        OnStaupAndTiming    // 启动时 + 定时同步
    }

    public class LogSetting
    {
        /// <summary>
        /// 启用服务
        /// </summary>
        public bool IsEnable { get; set; } = true;

        /// <summary>
        /// 最低日志级别
        /// </summary>
        public LogLevel MinLevel { get; set; } = LogLevel.Info;

        /// <summary>
        /// 输出格式
        /// </summary>
        public string OutputFormat { get; set; } = "${date:format=yyyy-MM-dd HH\\:mm\\:ss} [Uestc.BBS ${level}] ${message}${onexception:${newline}${exception:format=toString}${exception:format=StackTrace}}";
    }

    public class UpgradeSetting
    {
        /// <summary>
        /// 启用服务
        /// </summary>
        public bool AcceptBetaVersion { get; set; } = false;

        /// <summary>
        /// 更新地址
        /// </summary>
        public string Mirror { get; set; } = "https://mirrors.krins.cloud/Uestc.BBS";
    }

    [JsonSerializable(typeof(AppSetting))]
    public partial class AppSettingContext : JsonSerializerContext
    {
    }
}
