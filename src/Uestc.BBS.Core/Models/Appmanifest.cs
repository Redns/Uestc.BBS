using System.Text.Json.Serialization;

namespace Uestc.BBS.Core.Models
{
    public class Appmanifest
    {
        /// <summary>
        /// 应用名称
        /// </summary>
        public string Name { get; init; } = "清水河畔";

        /// <summary>
        /// 应用版本号
        /// </summary>
        public string Version { get; init; } = string.Empty;

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; init; } = "Redns";

        /// <summary>
        /// 开源协议
        /// </summary>
        public string License { get; init; } = "MIT";

        /// <summary>
        /// 项目起始日期
        /// </summary>
        public DateTime OriginalDate { get; init; } = DateTime.MinValue;

        /// <summary>
        /// 版权信息
        /// </summary>
        public string CopyRight =>
            OriginalDate.Year == DateTime.Now.Year
                ? $"©{OriginalDate.Year} {Author}. {License} License"
                : $"©{OriginalDate.Year}-{DateTime.Now.Year} {Author}. {License} License";

        /// <summary>
        /// 源码仓库
        /// </summary>
        public string SourceRepositoryUrl { get; init; } = "https://github.com/Redns/Uestc.BBS";

        /// <summary>
        /// 反馈与建议
        /// </summary>
        public string FeedbackUrl { get; init; } = "https://github.com/Redns/Uestc.BBS/issues/new";

        /// <summary>
        /// 贡献者列表
        /// </summary>
        public List<Contributor> Contributors { get; init; } = [];
    }

    [JsonSerializable(typeof(Appmanifest))]
    [JsonSourceGenerationOptions(WriteIndented = true, PropertyNameCaseInsensitive = true)]
    public partial class AppmanifestContext : JsonSerializerContext { }

    public class Contributor
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Avatar { get; set; } = string.Empty;

        public string Homepage { get; set; } = string.Empty;
    }
}
