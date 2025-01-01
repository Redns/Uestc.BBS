using System;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace Uestc.BBS.Desktop.Controls.AsyncImageLoader;

public interface IAsyncImageLoader : IDisposable
{
    /// <summary>
    /// 加载图片
    /// </summary>
    /// <param name="url">图片地址</param>
    /// <returns></returns>
    public Task<Bitmap?> ProvideImageAsync(string url);
}