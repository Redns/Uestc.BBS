using Avalonia.Media.Fonts;
using System;

namespace Uestc.BBS.Services
{
    public sealed class HarmonyOSFontCollection : EmbeddedFontCollection
    {
        public HarmonyOSFontCollection() : base(
            new Uri("fonts:HarmonyOS Sans", UriKind.Absolute),
            new Uri("avares://Uestc.BBS/Assets/Fonts", UriKind.Absolute))
        {
        }
    }
}
