using System;
using System.IO;
using Avalonia.Platform;

namespace Uestc.BBS.Desktop.Helpers
{
    public static class ResourceHelper
    {
        public static Stream Load(string relativeUrl)
        {
            return AssetLoader.Open(
                new Uri("avares://" + AppDomain.CurrentDomain.FriendlyName + relativeUrl)
            );
        }

        public static Stream Load(Uri uri) => AssetLoader.Open(uri);
    }
}
