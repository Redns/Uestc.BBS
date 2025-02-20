namespace Uestc.BBS.Core.Services.User
{
    public class UserService(HttpClient httpClient) : IUserService
    {
        private readonly HttpClient _httpClient = httpClient;


    }
}
