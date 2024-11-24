namespace Uestc.BBS.Core.Services.Api.User
{
    public class UserService(HttpClient httpClient) : IUserService
    {
        private readonly HttpClient _httpClient = httpClient;


    }
}
