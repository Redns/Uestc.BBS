using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Services.FileCache;
using Uestc.BBS.Sdk;

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
            if (uri.IsFile || uri.AbsoluteUri.StartsWith("ms-appx://"))
            {
                return uri;
            }

            // 网络图片
            // XXX AbsolutePath 不包含 Query 参数，头像链接包含 uid 参数
            var imageKey = GenerateUniqueKey(uri.PathAndQuery);
            var imageFullPath = Path.Combine(cacheRoot, imageKey);
            if (!File.Exists(imageFullPath))
            {
                // 缓存图片临时路径
                var imageTempPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.InternetCache),
                    imageKey
                );

                // 下载网络图片至本地缓存
                await _downloadTasks.GetOrAdd(
                    imageFullPath,
                    async _ =>
                    {
                        try
                        {
                            var client = httpClientFactory.CreateClient(ServiceExtensions.WEB_API);
                            using var imageReadStream =
                                await client.GetStreamAsync(uri, cancellationToken)
                                ?? throw new NullReferenceException(
                                    "Cache image from network failed, response is null."
                                );

                            using var imageWriteStream = new FileStream(
                                imageTempPath,
                                FileMode.Create,
                                FileAccess.Write,
                                FileShare.None,
                                80 * 1024,
                                FileOptions.Asynchronous | FileOptions.SequentialScan
                            );
                            await imageReadStream.CopyToAsync(imageWriteStream, cancellationToken);
                            imageWriteStream.Close();

                            // 避免缓存图片时出现异常导致缓存文件不完整
                            File.Move(imageTempPath, imageFullPath);
                        }
                        catch
                        {
                            File.Delete(imageTempPath);
                            throw;
                        }
                        finally
                        {
                            _downloadTasks.TryRemove(imageFullPath, out var _);
                        }
                    }
                );
            }
            return new Uri(imageFullPath);
        }

        public Task InvalidateAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            if (uri.IsFile || uri.AbsoluteUri.StartsWith("ms-appx://"))
            {
                return Task.CompletedTask;
            }

            // 网络图片
            // XXX AbsolutePath 不包含 Query 参数，头像链接包含 uid 参数
            var imageKey = GenerateUniqueKey(uri.PathAndQuery);
            var imageFullPath = Path.Combine(cacheRoot, imageKey);
            if (File.Exists(imageFullPath))
            {
                File.Delete(imageFullPath);
            }

            return Task.CompletedTask;
        }

        private string GenerateUniqueKey(string filename) => filename.ToMD5();
    }
}
