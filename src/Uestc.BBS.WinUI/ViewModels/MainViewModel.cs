using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services;
using Uestc.BBS.WinUI.Services.NavigateService;

namespace Uestc.BBS.WinUI.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        /// <summary>
        /// 调度任务队列
        /// </summary>
        private readonly DispatcherQueue _dispatcherQueue;

        /// <summary>
        /// 搜索栏提示文字更新定时器
        /// </summary>
        private readonly Timer _searchPlaceholderTextUpdateTimer;

        /// <summary>
        /// 应用设置
        /// </summary>
        private readonly AppSetting _appSetting;

        /// <summary>
        /// 导航服务
        /// </summary>
        private readonly INavigateService _navigateService;

        /// <summary>
        /// 每日一句
        /// </summary>
        private readonly IDailySentenceService _dailySentenceService;

        /// <summary>
        /// 搜索栏提示文字
        /// </summary>
        [ObservableProperty]
        public partial string SearchPlaceholderText { get; set; } = string.Empty;

        [ObservableProperty]
        public partial Page? Page { get; set; }

        public MainViewModel(
            AppSetting appSetting,
            INavigateService navigateService,
            IDailySentenceService dailySentenceService
        )
        {
            _appSetting = appSetting;
            _navigateService = navigateService;
            _dailySentenceService = dailySentenceService;
            _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            _searchPlaceholderTextUpdateTimer = new Timer(
                async state =>
                {
                    // 获取每日一句
                    // TODO 设置中可配置轮询频率
                    var sentence = await _dailySentenceService.GetDailySentenceAsync();
                    if (string.IsNullOrEmpty(sentence) || sentence == SearchPlaceholderText)
                    {
                        return;
                    }

                    await _dispatcherQueue.EnqueueAsync(() =>
                    {
                        SearchPlaceholderText = sentence;
                    });
                },
                null,
                0,
                60 * 1000
            );
        }
    }
}
