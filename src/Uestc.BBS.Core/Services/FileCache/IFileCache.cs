namespace Uestc.BBS.Core.Services.FileCache
{
    public interface IFileCache
    {
        /// <summary>
        /// 获取缓存文件 Uri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        ValueTask<Uri> GetFileUriAsync(Uri uri);
    }
}
