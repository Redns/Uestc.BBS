using System.Text.Json.Serialization;

namespace Uestc.BBS.Core.Services
{
    public class ApiRespBase
    {
        public int Rs { get; set; } = 0;

        public string ErrCode { get; set; } = string.Empty;

        public ApiRespBaseHeader Head { get; set; } = new();

        public ApiRespBaseBody Body { get; set; } = new();
    }

    public class ApiRespBaseHeader
    {
        public int Alert { get; set; } = 0;

        [JsonPropertyName("errCode")]
        public string ErrorCode { get; set; } = string.Empty;

        [JsonPropertyName("errInfo")]
        public string ErrorInformation { get; set; } = string.Empty;

        public string Version { get; set; } = string.Empty;
    }

    public class ApiRespBaseBody
    {
        public ApiRespBaseBodyExternInfo ExternInfo { get; set; } = new();
    }

    public class ApiRespBaseBodyExternInfo
    {
        public string Padding { get; set; } = string.Empty;
    }

    [JsonSerializable(typeof(ApiRespBase))]
    [JsonSourceGenerationOptions(
        WriteIndented = true,
        PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    )]
    public partial class ApiRespBaseContext : JsonSerializerContext { }
}
