using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Uestc.BBS.Core.Services.System
{
    public class Appmanifest
    {
        /// <summary>
        /// 应用名称
        /// </summary>
        public string Name { get; init; } = string.Empty;

        /// <summary>
        /// 应用版本号
        /// </summary>
        public string Version { get; init; } = string.Empty;

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; init; } = string.Empty;

        /// <summary>
        /// 开源协议
        /// </summary>
        public string License { get; init; } = string.Empty;

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
        public string SourceRepositoryUrl { get; init; } = string.Empty;

        /// <summary>
        /// 贡献者列表
        /// </summary>
        public IEnumerable<Contributor> Contributors { get; init; } = [];

        public static readonly JsonSerializerOptions SerializerOptions = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            TypeInfoResolver = JsonTypeInfoResolver.Combine(
                AppmanifestContext.Default,
                new DefaultJsonTypeInfoResolver()
            ),
        };
    }

    [JsonSerializable(typeof(Appmanifest))]
    public partial class AppmanifestContext : JsonSerializerContext { }

    public class Contributor
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Avatar { get; set; } = string.Empty;

        public string HomePage { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public static readonly JsonSerializerOptions SerializerOptions = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            TypeInfoResolver = JsonTypeInfoResolver.Combine(
                ContributorContext.Default,
                new DefaultJsonTypeInfoResolver()
            ),
        };
    }

    [JsonSerializable(typeof(Contributor))]
    public partial class ContributorContext : JsonSerializerContext { }
}
