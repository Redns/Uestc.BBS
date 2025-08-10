using System;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.ViewModels;
using Windows.Storage.Pickers;

namespace Uestc.BBS.WinUI.ViewModels
{
    public partial class StorageSettingViewModel(AppSettingModel appSettingModel)
        : StorageSettingViewModelBase(appSettingModel)
    {
        public override async void ChangeCacheRootDirectory()
        {
            var folderPicker = new FolderPicker();
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.CurrentWindow);

            // Initialize the folder picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hWnd);

            var ret = await folderPicker.PickSingleFolderAsync();
            if (ret != null)
            {
                StorageSettingModel.Cache.RootDirectory = ret.Path;
            }
        }
    }
}
