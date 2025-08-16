using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Messages;
using Uestc.BBS.Sdk.Services.Thread;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Views
{
    public sealed partial class HomePage : Page
    {
        private HomeViewModel ViewModel { get; init; }

        private readonly ILogService _logService;

        private readonly IThreadContentService _threadContentService;

        private CancellationTokenSource? _threadContentCancelTokenSource;

        public HomePage(
            HomeViewModel viewModel,
            ILogService logService,
            IThreadContentService threadContentService
        )
        {
            InitializeComponent();

            ViewModel = viewModel;
            ViewModel.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(viewModel.CurrentBoardTabItemListView))
                {
                    if (
                        viewModel.CurrentBoardTabItemListView.Topics!.IsLoading
                        || viewModel.CurrentBoardTabItemListView.Topics.Count is 0
                    )
                    {
                        return;
                    }

                    if (
                        viewModel.AppSettingModel.Appearance.BoardTab.Items.IndexOf(
                            viewModel.LastBoardTabItemModel!
                        )
                        > viewModel.AppSettingModel.Appearance.BoardTab.Items.IndexOf(
                            viewModel.CurrentBoardTabItemModel!
                        )
                    )
                    {
                        BoardSwitchLeft2RightStoryboard.Begin();
                        return;
                    }
                    BoardSwitchRight2LeftStoryboard.Begin();
                    return;
                }

                if (e.PropertyName == nameof(ViewModel.CurrentThread))
                {
                    ThreadContentScrollViewer.ScrollToVerticalOffset(0);
                }
            };

            _logService = logService;
            _threadContentService = threadContentService;

            // 注册主题选择消息
            StrongReferenceMessenger.Default.Register<ThreadChangedMessage>(
                this,
                async (_, m) =>
                {
                    // 如果当前正在加载的主题与消息中的主题相同，则不进行任何操作
                    // 如果当前并未加载主题，如果点击的是同一主题则仍然加载，等效于刷新
                    if (ViewModel.CurrentLoadingThreadId == m.Value)
                    {
                        return;
                    }
                    ViewModel.CurrentLoadingThreadId = m.Value;

                    // 取消上一次加载任务
                    _threadContentCancelTokenSource?.Cancel();
                    _threadContentCancelTokenSource?.Dispose();
                    _threadContentCancelTokenSource = new CancellationTokenSource();

                    try
                    {
                        // TODO 显示加载动画
                        // TODO 超时时间可配置
                        var threadContent = await _threadContentService
                            .GetThreadContentAsync(m.Value, _threadContentCancelTokenSource.Token)
                            .TimeoutCancelAsync(TimeSpan.FromSeconds(5));
                        await DispatcherQueue.EnqueueAsync(() =>
                        {
                            if (_threadContentCancelTokenSource?.IsCancellationRequested is true)
                            {
                                return;
                            }
                            ViewModel.CurrentThread = threadContent;
                        });
                    }
                    catch (TaskCanceledException) { }
                    catch (TimeoutException)
                    {
                        // TODO 显示超时信息
                    }
                    catch (Exception ex)
                    {
                        // TODO 显示错误信息

                        _logService.Error($"Failed to load thread {m.Value}", ex);
                    }
                    finally
                    {
                        ViewModel.CurrentLoadingThreadId = 0;
                    }
                }
            );
        }

        ~HomePage()
        {
            StrongReferenceMessenger.Default.Unregister<ThreadChangedMessage>(this);
        }

        /// <summary>
        /// 刷新当前板块的帖子列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_"></param>
        private async void RefreshBoardTabItems(object sender, PointerRoutedEventArgs _)
        {
            if (sender is not TextBlock textBlock)
            {
                return;
            }

            var clickedBoardTabItemModel =
                ViewModel.AppSettingModel.Appearance.BoardTab.Items.First(b =>
                    b.Name == textBlock.Text
                );

            if (clickedBoardTabItemModel != ViewModel.CurrentBoardTabItemModel)
            {
                return;
            }

            if (ViewModel.CurrentBoardTabItemListView!.Topics!.IsLoading)
            {
                return;
            }

            await ViewModel.CurrentBoardTabItemListView.Topics.RefreshAsync();
        }
    }
}
