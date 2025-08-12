using System.Text.Json.Serialization;
using FastEnumUtility;
using Uestc.BBS.Core.Services.System;

namespace Uestc.BBS.Core.Models
{
    public class ServicesSetting
    {
        /// <summary>
        /// 日志设置
        /// </summary>
        public LogSetting Log { get; set; } = new();

        /// <summary>
        /// API
        /// </summary>
        public NetworkSetting Network { get; set; } = new();

        /// <summary>
        /// 更新设置
        /// </summary>
        public UpgradeSetting Upgrade { get; set; } = new();

        /// <summary>
        /// 启动和关闭设置
        /// </summary>
        public StartupAndShutdownSetting StartupAndShutdown { get; set; } = new();
    }

    /// <summary>
    /// 日志设置
    /// </summary>
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
        public string OutputFormat { get; set; } =
            "${date:format=yyyy-MM-dd HH\\:mm\\:ss} [Uestc.BBS ${level}] ${message}${onexception:${newline}${exception:format=toString}${exception:format=StackTrace}}";

        /// <summary>
        /// 日志存档大小（MB）
        /// </summary>
        public long ArchiveAboveSize { get; set; } = 8;

        /// <summary>
        /// 最大日志文件数
        /// </summary>
        public int MaxArchiveFiles { get; set; } = 8;
    }

    public class NetworkSetting
    {
        /// <summary>
        /// 是否启用系统代理
        /// </summary>
        public bool UseSystemProxy { get; set; } = false;

        /// <summary>
        /// 是否启用证书验证
        /// </summary>
        public bool IsCertificateVerificationEnabled { get; set; } = true;

        /// <summary>
        /// 基地址
        /// </summary>
        public string BaseUrl
        {
            get;
            set
            {
                if (field != value)
                {
                    field = value;
                    BaseUri = new Uri(value);
                }
            }
        } = "https://bbs.uestc.edu.cn";

        [JsonIgnore]
        public Uri? BaseUri
        {
            get => field ??= new Uri(BaseUrl);
            private set;
        }

        // TODO 移除如下 URI 配置项，采用 Uestc.BBS.Sdk 统一配置
        /// <summary>
        /// 每日一句
        /// </summary>
        public string DailySentenceUrl
        {
            get;
            set
            {
                if (field != value)
                {
                    field = value;
                    DailySentenceUri = new Uri(BaseUri!, value);
                }
            }
        } = "forum.php?mobile=no";

        [JsonIgnore]
        public Uri? DailySentenceUri
        {
            get => field ??= new Uri(BaseUri!, DailySentenceUrl);
            private set;
        }

        /// <summary>
        /// 授权
        /// </summary>
        public string AuthUrl
        {
            get;
            set
            {
                if (field != value)
                {
                    field = value;
                    AuthUri = new Uri(BaseUri!, value);
                }
            }
        } = "mobcent/app/web/index.php?r=user/login";

        [JsonIgnore]
        public Uri? AuthUri
        {
            get => field ??= new Uri(BaseUri!, AuthUrl);
            private set;
        }

        /// <summary>
        /// 主题详情
        /// </summary>
        public string TopicDetailUrl
        {
            get;
            set
            {
                if (field != value)
                {
                    field = value;
                    TopicDetailUri = new Uri(BaseUri!, value);
                }
            }
        } = "mobcent/app/web/index.php?r=forum/postlist";

        [JsonIgnore]
        public Uri? TopicDetailUri
        {
            get => field ??= new Uri(BaseUri!, TopicDetailUrl);
            private set;
        }

        /// <summary>
        /// 主题列表
        /// </summary>
        public string TopicListUrl
        {
            get;
            set
            {
                if (field != value)
                {
                    field = value;
                    TopicListUri = new Uri(BaseUri!, value);
                }
            }
        } = "mobcent/app/web/index.php";

        [JsonIgnore]
        public Uri? TopicListUri
        {
            get => field ??= new Uri(BaseUri!, TopicListUrl);
            private set;
        }

        /// <summary>
        /// 用户详情
        /// </summary>
        public string UserDetailUrl
        {
            get;
            set
            {
                if (field != value)
                {
                    field = value;
                    UserDetailUri = new Uri(BaseUri!, value);
                }
            }
        } = "mobcent/app/web/index.php";

        [JsonIgnore]
        public Uri? UserDetailUri
        {
            get => field ??= new Uri(BaseUri!, UserDetailUrl);
            private set;
        }
    }

    /// <summary>
    /// 更新设置
    /// </summary>
    public class UpgradeSetting
    {
        /// <summary>
        /// 启用服务
        /// </summary>
        public bool AcceptBetaVersion { get; set; } = false;

        /// <summary>
        /// 上次更新检查时间
        /// </summary>
        public DateTime LastCheckTime { get; set; } = DateTime.MinValue;

        /// <summary>
        /// 更新地址
        /// </summary>
        public string Mirror { get; set; } = "https://mirrors.krins.cloud/Uestc.BBS";
    }

    /// <summary>
    /// 启动和关闭设置
    /// </summary>
    public class StartupAndShutdownSetting
    {
        /// <summary>
        /// 静默启动
        /// </summary>
        public bool SilentStart { get; set; } = false;

        /// <summary>
        /// 固定窗口
        /// </summary>
        public bool IsWindowPinned { get; set; } = false;

        /// <summary>
        /// 窗口关闭行为
        /// </summary>
        public WindowCloseBehavior WindowCloseBehavior { get; set; } = WindowCloseBehavior.Hide;
    }

    /// <summary>
    /// 窗口关闭行为
    /// </summary>
    public enum WindowCloseBehavior
    {
        [Label("退出应用")]
        Exit,

        [Label("隐藏窗口")]
        Hide,

        [Label("隐藏窗口 + 效率模式")]
        HideWithEfficiencyMode,
    }
}
