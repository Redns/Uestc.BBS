using System.Text.Json.Serialization;

namespace Uestc.BBS.Core.Models
{
    public class ApiSetting
    {
        /// <summary>
        /// 是否启用系统代理
        /// </summary>
        public bool IsSystemProxyEnabled { get; set; } = false;

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
}
