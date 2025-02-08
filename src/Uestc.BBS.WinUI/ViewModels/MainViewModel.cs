using System;
using System.Threading.Tasks;
using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Uestc.BBS.Core.Services;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.ViewModels;

namespace Uestc.BBS.WinUI.ViewModels
{
    public partial class MainViewModel : MainViewModelBase
    {
        /// <summary>
        /// 调度任务队列
        /// </summary>
        private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        /// <summary>
        /// 顶部导航栏可见性
        /// </summary>
        public Visibility TopNavigateBarVisibility =>
            AppSettingModel.Apperance.IsTopNavigateBarEnabled
                ? Visibility.Visible
                : Visibility.Collapsed;

        public MainViewModel(
            AppSettingModel appSettingModel,
            IDailySentenceService dailySentenceService
        )
            : base(appSettingModel, dailySentenceService)
        {
            AppSettingModel.Apperance.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(AppSettingModel.Apperance.IsTopNavigateBarEnabled))
                {
                    OnPropertyChanged(nameof(TopNavigateBarVisibility));
                }
            };
        }

        public override Task DispatcherAsync(Action action) =>
            _dispatcherQueue.EnqueueAsync(action);
    }
}
