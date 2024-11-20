using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Uestc.BBS.Core.Services.Api
{
    public class ApiRespBase
    {
        public int Rs { get; set; } = 0;

        public string ErrCode {  get; set; } = string.Empty;

        public ApiRespBaseHeader Header { get; set; } = new();

        public ApiRespBaseBody Body { get; set; } = new();

        private static readonly JsonSerializerOptions _serializerOptions = new()
        {
            WriteIndented = true,
            TypeInfoResolver = JsonTypeInfoResolver.Combine(ApiRespBaseContext.Default, new DefaultJsonTypeInfoResolver())
        };
    }

    public class ApiRespBaseHeader
    {
        public int Alert { get; set; } = 0;

        public string ErrCode { get; set; } = string.Empty;

        public string ErrInfo {  get; set; } = string.Empty;

        public string Version {  get; set; } = string.Empty;
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
    public partial class ApiRespBaseContext : JsonSerializerContext
    {
    }
}
