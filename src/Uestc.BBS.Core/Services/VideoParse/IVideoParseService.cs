namespace Uestc.BBS.Core.Services.VideoParse
{
    public interface IVideoParseService
    {
        Task<Uri> GetSourceUriAsync(string url);
    }
}
