using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace Uestc.BBS.Desktop.Helpers
{
    public static class ImageHelper
    {
        public static Bitmap LoadFromResource(string relativeUrl) =>
            new(ResourceHelper.Load(relativeUrl));

        public static async Task<Bitmap?> LoadFromWebAsync(HttpClient client, string? url)
        {
            return string.IsNullOrEmpty(url)
                ? null
                : new Bitmap(new MemoryStream(await client.GetByteArrayAsync(url)));
        }
    }
}
