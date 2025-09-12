namespace Uestc.BBS.Core.Services.FileCache
{
    public interface IFileCache
    {
        /// <summary>
        /// 获取缓存文件 Uri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        Task<Uri> GetFileUriAsync(Uri uri, CancellationToken cancellationToken = default);

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task InvalidateAsync(Uri uri, CancellationToken cancellationToken = default);

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ClearAsync(CancellationToken cancellationToken = default);
    }
}
