using Avalonia.Media.Fonts;
using System;

namespace Uestc.BBS.Desktop.Services
{
    public sealed class HarmonyOSFontCollection : EmbeddedFontCollection
    {
        public HarmonyOSFontCollection() : base(
            new Uri("fonts:HarmonyOS Sans", UriKind.Absolute),
            new Uri("avares://Uestc.BBS.Desktop/Resources/Fonts", UriKind.Absolute))
        {
        }
    }
}
