using System.Text.Json.Serialization;
using FastEnumUtility;

namespace Uestc.BBS.Core.Services.Forum
{
    public class ExtraPanel
    {
        public string Title { get; set; } = string.Empty;

        public ExtarPanelType Type { get; set; }

        public string Action { get; set; } = string.Empty;

        public string RecommendAdd { get; set; } = string.Empty;

        [JsonPropertyName("extParams")]
        public ExtraParam ExtraParam { get; set; } = new();
    }

    public class ExtraParam
    {
        public uint RecommendAdd { get; set; }

        public bool IsHasRecommendAdd { get; set; }

        public string BeforeAction { get; set; } = string.Empty;
    }

    public enum ExtarPanelType
    {
        [Label("评分")]
        Rate,

        [Label("支持")]
        Support,
    }
}
