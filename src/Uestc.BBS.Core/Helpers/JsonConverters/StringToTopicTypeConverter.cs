using System.Text.Json.Serialization;
using System.Text.Json;
using Uestc.BBS.Core.Services.Api.Forum;

namespace Uestc.BBS.Core.Helpers.JsonConverters
{
    public class StringToTopicTypeConverter : JsonConverter<TopicType>
    {
        public override TopicType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String && Enum.TryParse(typeof(TopicType), reader.GetString(), true, out var value))
            {
                return (TopicType)value;
            }
            throw new JsonException("Expected a number value for boolean.");
        }

        public override void Write(Utf8JsonWriter writer, TopicType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
