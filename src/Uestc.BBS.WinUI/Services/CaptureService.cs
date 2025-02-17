using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Uestc.BBS.Mvvm.Services;
using Uestc.BBS.WinUI.Helpers;

namespace Uestc.BBS.WinUI.Services
{
    public class CaptureService : ICaptureService<UIElement>
    {
        public Task Capture(UIElement element, string filePath) =>
            element.CaptureControlAsImageAsync(filePath);
    }
}
