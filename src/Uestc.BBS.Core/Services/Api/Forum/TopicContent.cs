using System.Text.Json.Serialization;
using FastEnumUtility;

namespace Uestc.BBS.Core.Services.Api.Forum
{
    public class TopicContent
    {
        [JsonPropertyName("infor")]
        public string Information { get; set; } = string.Empty;

        public TopicContenType Type { get; set; } = TopicContenType.Text;

        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("originalInfor")]
        public string OriginalInformation { get; set; } = string.Empty;

        public uint Aid { get; set; }
    }

    public enum TopicContenType
    {
        [Label("文本")]
        Text = 0,

        [Label("图片")]
        Image = 1,

        [Label("内联链接")]
        InlineLink = 4
    }
}
