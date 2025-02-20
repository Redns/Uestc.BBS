using System.Text.Json.Serialization;
using FastEnumUtility;

namespace Uestc.BBS.Core.Services.Forum
{
    public class RichTextContent
    {
        /// <summary>
        /// 内容
        /// </summary>
        [JsonPropertyName("infor")]
        public string Information { get; set; } = string.Empty;

        /// <summary>
        /// 类型
        /// </summary>
        public TopicContenType Type { get; set; } = TopicContenType.Text;

        /// <summary>
        ///
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// 渲染图片请使用此字段
        /// </summary>
        [JsonPropertyName("originalInfor")]
        public string OriginalInformation { get; set; } = string.Empty;

        /// <summary>
        ///
        /// </summary>
        public uint Aid { get; set; }

        /// <summary>
        /// 描述（19.36 KB, 下载次数: 1）
        /// </summary>
        [JsonPropertyName("desc")]
        public string Description { get; set; } = string.Empty;
    }

    public enum TopicContenType
    {
        [Label("文本")]
        Text = 0,

        [Label("图片")]
        Image = 1,

        [Label("内联链接")]
        InlineLink = 4,

        [Label("附件")]
        Attachment = 5
    }
}
