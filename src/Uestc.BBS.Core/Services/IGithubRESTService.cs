using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Uestc.BBS.Core.Services
{
    public interface IGithubRESTService
    {
        Task<IEnumerable<GithubUser>> GetContributorsAsync(string owner, string repo);
    }

    public class GithubUser
    {
        public uint Id { get; set; } = 0;

        public string Login { get; set; } = string.Empty;

        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; } = string.Empty;

        [JsonPropertyName("html_url")]
        public string HtmlUrl { get; set; } = string.Empty;

        public uint Contributions { get; set; } = 0;

        public static readonly JsonSerializerOptions SerializerOptions = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            TypeInfoResolver = JsonTypeInfoResolver.Combine(
                GithubUserContext.Default,
                new DefaultJsonTypeInfoResolver()
            ),
        };
    }

    [JsonSerializable(typeof(GithubUser))]
    public partial class GithubUserContext : JsonSerializerContext { }
}
