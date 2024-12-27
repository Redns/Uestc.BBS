namespace Uestc.BBS.Core.Services.Api.User
{
    public partial class User { }

    public static class UserExtension
    {
        public static uint GetUserTitleLevel(this string userTitle)
        {
            if (uint.TryParse(userTitle.Split('.').Last()[0..^1], out var level))
            {
                return level;
            }

            throw new ArgumentException("Unable to parse level from user title", nameof(userTitle));
        }

        public static string GetUserTitleAlias(this string userTitle) =>
            userTitle.Split('（').First();
    }
}
