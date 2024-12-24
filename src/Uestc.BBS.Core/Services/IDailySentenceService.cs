namespace Uestc.BBS.Core.Services
{
    public interface IDailySentenceService
    {
        Task<string> GetDailySentenceAsync();
    }
}
