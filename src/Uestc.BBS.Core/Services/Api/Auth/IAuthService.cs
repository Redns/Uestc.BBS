namespace Uestc.BBS.Core.Services.Api.Auth
{
    public interface IAuthService
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        Task<AuthResp?> LoginAsync(string username, string password);
    }
}
