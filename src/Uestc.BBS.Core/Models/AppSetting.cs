using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Text.Unicode;
using Microsoft.Extensions.DependencyInjection;
using Uestc.BBS.Core.Services.System;

namespace Uestc.BBS.Core.Models
{
    public class AppSetting
    {
        private static readonly string _defalutStoragePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            AppDomain.CurrentDomain.FriendlyName,
            "appsettings.json"
        );

        /// <summary>
        /// 外观
        /// </summary>
        public AppearanceSetting Appearance { get; set; } = new();

        /// <summary>
        /// 浏览
        /// </summary>
        public BrowseSetting Browse { get; set; } = new();

        /// <summary>
        /// 授权
        /// </summary>
        public AccountSetting Account { get; set; } = new();

        /// <summary>
        /// 通知
        /// </summary>
        public NotificationSetting Notification { get; set; } = new();

        /// <summary>
        /// 服务
        /// </summary>
        public ServicesSetting Services { get; set; } = new();

        /// <summary>
        /// 数据与存储
        /// </summary>
        public StorageSetting Storage { get; set; } = new();

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
                path ??= _defalutStoragePath;
                if (File.Exists(path))
                {
                    appSetting = JsonSerializer.Deserialize<AppSetting>(
                        File.ReadAllText(path),
                        SerializerOptions
                    );

                    if (appSetting is not null)
                    {
                        return appSetting;
                    }
                }
            }
            catch (Exception e)
            {
                ServiceExtension
                    .Services.GetRequiredService<ILogService>()
                    .Error(string.Format("Appsetting file {0} load failed", path), e);
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
            File.WriteAllText(path ?? _defalutStoragePath, ToString());
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, SerializerOptions);
        }

        public static readonly JsonSerializerOptions SerializerOptions = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            TypeInfoResolver = JsonTypeInfoResolver.Combine(
                AppSettingContext.Default,
                new DefaultJsonTypeInfoResolver()
            ),
        };
    }

    [JsonSerializable(typeof(AppSetting))]
    public partial class AppSettingContext : JsonSerializerContext { }
}
