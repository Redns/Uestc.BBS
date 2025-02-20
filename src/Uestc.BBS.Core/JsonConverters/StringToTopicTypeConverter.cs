using System.Text.Json;
using System.Text.Json.Serialization;
using FastEnumUtility;
using Uestc.BBS.Core.Services.Forum;

namespace Uestc.BBS.Core.JsonConverters
{
    public class StringToTopicTypeConverter : JsonConverter<TopicType>
    {
        public override TopicType Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            if (reader.TokenType is not JsonTokenType.String)
            {
                throw new JsonException("Expected a number value for boolean");
            }

            return FastEnum.Parse<TopicType>(reader.GetString(), true);
        }

        public override void Write(
            Utf8JsonWriter writer,
            TopicType value,
            JsonSerializerOptions options
        )
        {
            writer.WriteStringValue(value.FastToString());
        }
    }
}
