using System.Text.Json.Serialization;
using System.Text.Json;

namespace Uestc.BBS.Core.Helpers.JsonConverters
{
    public class UintToBoolConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetUInt32() > 0;
            }
            throw new JsonException("Expected a number value for boolean.");
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value ? 1 : 0);
        }
    }
}
