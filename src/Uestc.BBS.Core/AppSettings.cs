using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Services;

namespace Uestc.BBS.Core
{
    public class AppSetting
    {
        public AuthSetting Auth { get; set; } = new();

        public SyncSetting Sync { get; set; } = new();

        public static AppSetting Load(string path, string? secret = null)
        {
            AppSetting? appSetting;

            try
            {
                if (File.Exists(path))
                {
                    var decryptSetting = File.ReadAllText(path).Decrypt(secret);
                    appSetting = JsonSerializer.Deserialize<AppSetting>(decryptSetting, _serializerOptions);
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
            appSetting.Save(path, secret);

            return appSetting;
        }

        public void Save(string path, string? secret = null)
        {
            File.WriteAllText(path, ToString().Encrypt(secret));
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, _serializerOptions);
        }

        private static readonly JsonSerializerOptions _serializerOptions = new()
        {
            WriteIndented = true,
            TypeInfoResolver = JsonTypeInfoResolver.Combine(AppSettingContext.Default, new DefaultJsonTypeInfoResolver())
        };
    }

    public class AuthSetting
    {
        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public bool AutoLogin { get; set; } = false;

        public bool RememberPassword { get; set; } = false;

        public uint DefaultCredentialUid {  get; set; }

        [JsonIgnore]
        public AuthCredential? DefaultCredential
        {
            get
            {
                return Credentials.FirstOrDefault(c => c.Uid == DefaultCredentialUid);
            }
        }

        public IEnumerable<AuthCredential> Credentials { get; set; } = [];
    }

    public class AuthCredential
    {
        public uint Uid {  get; set; }

        public string Name { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public string Secret { get; set; } = string.Empty;

        public string Avatar { get; set; } = string.Empty;
    }

    /// <summary>
    /// 同步设置
    /// </summary>
    public class SyncSetting
    {
        /// <summary>
        /// 同步密钥
        /// </summary>
        public string Secret { get; set; } = string.Empty;

        /// <summary>
        /// 同步模式
        /// </summary>
        public SyncMode SyncMode { get; set; } = SyncMode.Timing;

        /// <summary>
        /// 最近更新日期
        /// 该日期用于判断是否同步云端设置至本地，在某些情况下用户设备 ID 更改导致本地设置丢失，如果
        /// 该属性默认值为 DateTime.Now，则默认设置将覆盖云端设置
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 更新时间间隔
        /// </summary>
        public TimeSpan UpdateTimeInterval { get; set; } = TimeSpan.FromMinutes(60);
    }

    /// <summary>
    /// 同步模式
    /// </summary>
    public enum SyncMode
    {
        None = 0,       // 手动同步
        OnStartup,      // 应用启动时同步
        Timing          // 定时同步
    }

    [JsonSerializable(typeof(AppSetting))]
    public partial class AppSettingContext : JsonSerializerContext
    {
    }
}
