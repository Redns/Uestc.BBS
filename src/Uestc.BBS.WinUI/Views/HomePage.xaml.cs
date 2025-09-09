using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Messages;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Sdk;
using Uestc.BBS.Sdk.Services.Thread.ThreadContent;
using Uestc.BBS.WinUI.ViewModels;
using Uestc.BBS.WinUI.Views.ContentDialogs;
using Windows.System;
using WinUIEx;

namespace Uestc.BBS.WinUI.Views
{
    public sealed partial class HomePage : Page
    {
        private readonly ILogService _logService;

        private readonly IThreadContentService _threadContentService;

        private HomeViewModel ViewModel { get; init; }

        private CancellationTokenSource? _threadContentCancelTokenSource;

        public HomePage(
            HomeViewModel viewModel,
            ILogService logService,
            [FromKeyedServices(ServiceExtensions.MOBCENT_API)]
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
                        viewModel.CurrentBoardTabItemListView.Threads!.IsLoading
                        || viewModel.CurrentBoardTabItemListView.Threads.Count is 0
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
                    // 滚动至顶部
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
                        ViewModel.PageStatus = PageStatus.Loading;

                        // TODO 超时时间可配置
                        var threadContent = await _threadContentService
                            .GetThreadContentAsync(
                                m.Value,
                                page: 1,
                                pageSize: 100,
                                cancellationToken: _threadContentCancelTokenSource.Token
                            )
                            .TimeoutCancelAsync(TimeSpan.FromSeconds(5));

                        if (threadContent.Contents.Length is 0)
                        {
                            return;
                        }

                        if (EmploymentWarningRegex().IsMatch(threadContent.Contents[0].Information))
                        {
                            threadContent.Contents = threadContent.Contents[1..];
                        }

                        await DispatcherQueue.EnqueueAsync(() =>
                        {
                            if (_threadContentCancelTokenSource?.IsCancellationRequested is true)
                            {
                                return;
                            }
                            ViewModel.CurrentThread = threadContent;
                            ViewModel.PageStatus = PageStatus.Success;
                        });
                    }
                    catch (TaskCanceledException) { }
                    catch (TimeoutException)
                    {
                        ViewModel.PageStatus = PageStatus.Timeout;
                    }
                    catch (Exception ex)
                    {
                        ViewModel.PageStatus = PageStatus.Error;

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
        /// <param name="_"></param>
        /// <param name="e"></param>
        private async void RefreshBoardTabItem(object _, ItemClickEventArgs e)
        {
            if (e.ClickedItem is not BoardTabItemModel boardTabItemModel)
            {
                return;
            }

            if (boardTabItemModel != ViewModel.CurrentBoardTabItemModel)
            {
                return;
            }

            if (ViewModel.CurrentBoardTabItemListView!.Threads!.IsLoading)
            {
                return;
            }

            await ViewModel.CurrentBoardTabItemListView.Threads.RefreshAsync();
        }

        /// <summary>
        /// 就业区主题内容会包含警告信息
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(
            @"^⚠️⚠️就业区仅限内部交流！严禁外传！⚠️⚠️\r\n------------(\w+)-(\w+)------------$",
            RegexOptions.Multiline
        )]
        private static partial Regex EmploymentWarningRegex();

        private async void EnterToReply(
            KeyboardAccelerator _,
            KeyboardAcceleratorInvokedEventArgs __
        )
        {
            if (string.IsNullOrEmpty(ViewModel.ReplyContent) || ViewModel.ReplyCommand.IsRunning)
            {
                return;
            }

            await ViewModel.ReplyAsync();
        }

        [RelayCommand]
        private void OpenReplyDialog()
        {
            var window = new WindowEx
            {
                ExtendsContentIntoTitleBar = true,
                SystemBackdrop = new MicaBackdrop(),
                PresenterKind = AppWindowPresenterKind.CompactOverlay,
                Content = new ScrollViewer
                {
                    ZoomMode = ZoomMode.Enabled,
                    Content = new ThreadReplyDialog
                    {
                        Uid = ViewModel.AppSettingModel.Account.DefaultCredential!.Uid,
                    },
                    VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
                },
            };

            window.SetWindowSize(800, 600);
            window.Activate();
            window.CenterOnScreen();
        }
    }
}
