using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Desktop.Helpers;

namespace Uestc.BBS.Desktop.Models
{
    public class ContributorModel : Contributor
    {
        public Task<Bitmap> AvatarImage => Task.FromResult(ImageHelper.LoadFromResource(Avatar));
    }
}
