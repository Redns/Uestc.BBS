using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Mvvm.Models;

namespace Uestc.BBS.Mvvm.ViewModels
{
    public abstract partial class StorageSettingViewModelBase(AppSettingModel appSettingModel)
        : ObservableObject
    {
        public StorageSettingModel StorageSettingModel { get; } = appSettingModel.Storage;

        /// <summary>
        /// 缓存总大小（字节）
        /// </summary>
        /// <returns></returns>
        public long CacheTotalSize =>
            StorageSettingModel.Cache.RootDirectory.GetFileTotalSize(
                "*",
                SearchOption.TopDirectoryOnly
            );

        /// <summary>
        /// 打开缓存根目录
        /// </summary>
        [RelayCommand]
        private void OpenCacheRootDirectory() =>
            OperatingSystemHelper.OpenDirectory(appSettingModel.Storage.Cache.RootDirectory);

        /// <summary>
        /// 更改缓存根目录
        /// </summary>
        [RelayCommand]
        public abstract Task ChangeCacheRootDirectoryAsync();

        /// <summary>
        /// 清空缓存
        /// </summary>
        [RelayCommand]
        public abstract Task ClearCacheAsync();
    }
}
