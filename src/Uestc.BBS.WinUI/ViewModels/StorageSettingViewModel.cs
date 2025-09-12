using System;
using System.IO;
using System.Threading.Tasks;
using Uestc.BBS.Core.Services.FileCache;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.ViewModels;
using Windows.Storage.Pickers;

namespace Uestc.BBS.WinUI.ViewModels
{
    public partial class StorageSettingViewModel(
        ILogService logService,
        IFileCache fileCacheService,
        INotificationService notificationService,
        AppSettingModel appSettingModel
    ) : StorageSettingViewModelBase(appSettingModel)
    {
        /// <summary>
        /// 更换缓存目录
        /// </summary>
        public override async Task ChangeCacheRootDirectoryAsync()
        {
            try
            {
                var folderPicker = new FolderPicker()
                {
                    SuggestedStartLocation = PickerLocationId.ComputerFolder,
                };
                var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.CurrentWindow);
                // Initialize the folder picker with the window handle (HWND).
                WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hWnd);

                // 选择缓存目录
                var ret = await folderPicker.PickSingleFolderAsync();
                if (ret is null || ret.Path == StorageSettingModel.Cache.RootDirectory)
                {
                    return;
                }

                // 移动缓存目录
                Directory.Move(StorageSettingModel.Cache.RootDirectory, ret.Path);
                StorageSettingModel.Cache.RootDirectory = ret.Path;
            }
            catch (Exception ex)
            {
                notificationService.Show("缓存路径更换失败", ex.Message);
                logService.Error("Failed to change cache root directory.", ex);
            }
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public override async Task ClearCacheAsync()
        {
            try
            {
                await fileCacheService.ClearAsync();
                OnPropertyChanged(nameof(CacheTotalSize));
            }
            catch (Exception ex)
            {
                notificationService.Show("缓存清除失败", ex.Message);
                logService.Error("Failed to clear cache.", ex);
            }
        }
    }
}
