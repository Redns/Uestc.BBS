using System;
using System.Threading;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml.Controls;
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
            };

            _logService = logService;
            _threadContentService = threadContentService;

            // 注册主题选择消息
            StrongReferenceMessenger.Default.Register<ThreadChangedMessage>(
                this,
                async (_, m) =>
                {
                    try
                    {
                        _threadContentCancelTokenSource?.Cancel();
                        _threadContentCancelTokenSource?.Dispose();
                        _threadContentCancelTokenSource = new CancellationTokenSource();

                        var threadContent = await _threadContentService.GetThreadContentAsync(
                            m.Value,
                            _threadContentCancelTokenSource.Token
                        );
                        await DispatcherQueue.EnqueueAsync(() =>
                        {
                            if (_threadContentCancelTokenSource?.IsCancellationRequested is true)
                            {
                                return;
                            }
                            ViewModel.CurrentThread = threadContent;
                        });
                    }
                    catch (OperationCanceledException ex)
                    {
                        if (_threadContentCancelTokenSource?.IsCancellationRequested is true)
                        {
                            return;
                        }

                        _logService.Error(
                            $"Failed to get thread (id: {m.Value}) content, task is canceled",
                            ex
                        );
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(
                            $"获取主题 {m.Value} 内容失败, {ex.Message}"
                        );
                    }
                }
            );
        }

        ~HomePage()
        {
            StrongReferenceMessenger.Default.Unregister<ThreadChangedMessage>(this);
        }
    }
}
