using System;
using System.Threading.Tasks;
using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;
using Uestc.BBS.Core.Services;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.ViewModels;

namespace Uestc.BBS.WinUI.ViewModels
{
    public partial class MainViewModel(
        AppSettingModel appSettingModel,
        IDailySentenceService dailySentenceService
    ) : MainViewModelBase(appSettingModel, dailySentenceService)
    {
        /// <summary>
        /// 调度任务队列
        /// </summary>
        private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        public override Task DispatcherAsync(Action action) =>
            _dispatcherQueue.EnqueueAsync(action);
    }
}
