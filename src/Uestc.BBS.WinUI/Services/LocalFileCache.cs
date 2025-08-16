using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Services.FileCache;

namespace Uestc.BBS.WinUI.Services
{
    public class LocalFileCache(string cacheRoot, IHttpClientFactory httpClientFactory) : IFileCache
    {
        /// <summary>
        /// 防止重复下载
        /// </summary>
        private static readonly ConcurrentDictionary<string, Task> _downloadTasks = new();

        /// <summary>
        /// 获取缓存文件 Uri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async Task<Uri> GetFileUriAsync(
            Uri uri,
            CancellationToken cancellationToken = default
        )
        {
            if (uri.IsFile)
            {
                return uri;
            }

            // 网络图片
            // XXX AbsolutePath 不包含 Query 参数，头像链接包含 uid 参数
            var imageFullPath = Path.Combine(cacheRoot, uri.PathAndQuery.ToMD5());
            if (!File.Exists(imageFullPath))
            {
                // 下载网络图片至本地缓存
                await _downloadTasks.GetOrAdd(
                    imageFullPath,
                    async _ =>
                    {
                        try
                        {
                            var client = httpClientFactory.CreateClient();
                            using var imageReadStream =
                                await client.GetStreamAsync(uri, cancellationToken)
                                ?? throw new NullReferenceException(
                                    "Cache image from network failed, response is null."
                                );

                            using var imageWriteStream = new FileStream(
                                imageFullPath,
                                FileMode.Create,
                                FileAccess.Write,
                                FileShare.None
                            );
                            await imageReadStream.CopyToAsync(imageWriteStream, cancellationToken);
                            await imageWriteStream.FlushAsync(cancellationToken);
                        }
                        catch
                        {
                            _downloadTasks.TryRemove(imageFullPath, out var _);
                            File.Delete(imageFullPath);
                            throw;
                        }
                    }
                );
            }
            return new Uri(imageFullPath);
        }
    }
}
