using System.Text.Json;
using Uestc.BBS.Core.Services.Api.Auth;

namespace Uestc.BBS.Core.Services.Auth
{
    public class AuthService(HttpClient httpClient) : IAuthService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<AuthResp?> LoginAsync(string username, string password)
        {
            using var resp = await _httpClient
                .PostAsync(
                    string.Empty,
                    new FormUrlEncodedContent(
                        new Dictionary<string, string>
                        {
                            { nameof(username), username },
                            { nameof(password), password }
                        }
                    )
                )
                .ContinueWith(t => t.Result.EnsureSuccessStatusCode());

            return await JsonSerializer.DeserializeAsync(
                await resp.Content.ReadAsStreamAsync(),
                AuthRespContext.Default.AuthResp
            );
        }
    }
}
