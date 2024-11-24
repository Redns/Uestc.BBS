using System.Net;
using System.Text.Json;

namespace Uestc.BBS.Core.Services.Api.Auth
{
    public class AuthService(HttpClient httpClient) : IAuthService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<AuthResp?> LoginAsync(string username, string password)
        {
            using var resp = await _httpClient.PostAsync(string.Empty, new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { nameof(username), username },
                { nameof(password), password }
            }));

            if (resp.StatusCode is not HttpStatusCode.OK)
            {
                return null;
            }

            return await JsonSerializer.DeserializeAsync<AuthResp>(await resp.Content.ReadAsStreamAsync(), AuthResp.SerializerOptions);
        }
    }
}
