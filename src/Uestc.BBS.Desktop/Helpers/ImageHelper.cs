using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace Uestc.BBS.Desktop.Helpers
{
    public static class ImageHelper
    {
        public static Bitmap LoadFromResource(Uri resourceUri)
        {
            return new Bitmap(AssetLoader.Open(resourceUri));
        }

        public static async Task<Bitmap?> LoadFromWeb(string? url)
        {
            using var client = new HttpClient();
            return await LoadFromWeb(client, url);
        }

        public static async Task<Bitmap?> LoadFromWeb(HttpClient client, string? url)
        {
            return string.IsNullOrEmpty(url) ? null : new Bitmap(new MemoryStream(await client.GetByteArrayAsync(url)));
        }
    }
}
