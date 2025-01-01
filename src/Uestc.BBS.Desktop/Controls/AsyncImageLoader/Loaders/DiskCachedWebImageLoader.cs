using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace Uestc.BBS.Desktop.Controls.AsyncImageLoader.Loaders;

/// <summary>
///     Provides memory and disk cached way to asynchronously load images for <see cref="ImageLoader" />
///     Can be used as base class if you want to create custom caching mechanism
/// </summary>
public class DiskCachedWebImageLoader : RamCachedWebImageLoader
{
    private readonly string _cacheFolder;

    public DiskCachedWebImageLoader(string? cacheFolder = null)
    {
        _cacheFolder =
            cacheFolder ?? Path.Combine(Path.GetTempPath(), AppDomain.CurrentDomain.FriendlyName);
    }

    public DiskCachedWebImageLoader(
        HttpClient httpClient,
        bool disposeHttpClient,
        string? cacheFolder = null
    )
        : base(httpClient, disposeHttpClient)
    {
        _cacheFolder =
            cacheFolder ?? Path.Combine(Path.GetTempPath(), AppDomain.CurrentDomain.FriendlyName);
    }

    /// <inheritdoc />
    protected override Task<Bitmap?> LoadFromGlobalCache(string url)
    {
        var path = Path.Combine(_cacheFolder, CreateMD5(url));

        return File.Exists(path)
            ? Task.FromResult<Bitmap?>(new Bitmap(path))
            : Task.FromResult<Bitmap?>(null);
    }

#if NETSTANDARD2_1
    protected override async Task SaveToGlobalCache(string url, byte[] imageBytes)
    {
        var path = Path.Combine(_cacheFolder, CreateMD5(url));

        Directory.CreateDirectory(_cacheFolder);
        await File.WriteAllBytesAsync(path, imageBytes).ConfigureAwait(false);
    }
#else
    protected override async Task SaveToGlobalCache(string url, byte[] imageBytes)
    {
        var path = Path.Combine(_cacheFolder, CreateMD5(url));
        if (!Directory.Exists(_cacheFolder))
        {
            Directory.CreateDirectory(_cacheFolder);
        }
        await File.WriteAllBytesAsync(path, imageBytes);
    }
#endif

    protected static string CreateMD5(string input)
    {
        // Use input string to calculate MD5 hash
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = MD5.HashData(inputBytes);

        // Convert the byte array to hexadecimal string
        return BitConverter.ToString(hashBytes).Replace("-", string.Empty);
    }
}
