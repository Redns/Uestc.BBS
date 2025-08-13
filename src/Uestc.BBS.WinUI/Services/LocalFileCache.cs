using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Services.FileCache;

namespace Uestc.BBS.WinUI.Services
{
    public class LocalFileCache(string cacheRoot, IHttpClientFactory httpClientFactory) : IFileCache
    {
        /// <summary>
        /// 缓存根目录
        /// </summary>
        private readonly string _cacheRoot = cacheRoot;

        /// <summary>
        /// HttpClient
        /// </summary>
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        /// <summary>
        /// 防止重复下载
        /// </summary>
        private static readonly ConcurrentDictionary<string, Task> _downloadTasks = new();

        /// <summary>
        /// 获取缓存文件 Uri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async Task<Uri> GetFileUriAsync(Uri uri)
        {
            if (uri.IsFile || uri.Scheme == "ms-appx" || uri.Scheme == "ms-appdata")
            {
                return uri;
            }

            // 网络图片
            // XXX AbsolutePath 不包含 Query 参数，头像链接包含 uid 参数
            var imageFullPath = Path.Combine(_cacheRoot, uri.PathAndQuery.ToMD5());
            if (!File.Exists(imageFullPath))
            {
                // 下载网络图片至本地缓存
                await _downloadTasks.GetOrAdd(
                    imageFullPath,
                    async _ =>
                    {
                        var bytes = await _httpClientFactory.CreateClient().GetByteArrayAsync(uri);
                        await File.WriteAllBytesAsync(imageFullPath, bytes);
                    }
                );
            }
            return new Uri(imageFullPath);
        }
    }
}
