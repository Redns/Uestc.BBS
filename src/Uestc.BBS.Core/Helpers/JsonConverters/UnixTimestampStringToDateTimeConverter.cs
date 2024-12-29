using System.Text.Json;
using System.Text.Json.Serialization;

namespace Uestc.BBS.Core.Helpers.JsonConverters
{
    public class UnixTimestampStringToDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            if (
                reader.TokenType is JsonTokenType.String
                && long.TryParse(reader.GetString(), out var timestamp)
            )
            {
                return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).LocalDateTime;
            }

            throw new JsonException("Expected a String value for DateTime.");
        }

        public override void Write(
            Utf8JsonWriter writer,
            DateTime value,
            JsonSerializerOptions options
        )
        {
            writer.WriteStringValue(value);
        }
    }
}
