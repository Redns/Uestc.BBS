using System.Net;
using System.Text.Json;

namespace Uestc.BBS.Core.Services
{
    public class GithubRESTService : IGithubRESTService
    {
        private readonly HttpClient _httpClient;

        public GithubRESTService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<GithubUser>> GetContributorsAsync(string owner, string repo)
        {
            if (string.IsNullOrEmpty(owner) || string.IsNullOrEmpty(repo))
            {
                return [];
            }

            using var resp = await _httpClient.GetAsync($"repos/{owner}/{repo}/contributors");
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                return [];
            }

            return await JsonSerializer.DeserializeAsync<IEnumerable<GithubUser>>(
                    await resp.Content.ReadAsStreamAsync(),
                    GithubUser.SerializerOptions
                ) ?? [];
        }
    }
}
