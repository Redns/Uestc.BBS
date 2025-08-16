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

            // ע������ѡ����Ϣ
            StrongReferenceMessenger.Default.Register<ThreadChangedMessage>(
                this,
                async (_, m) =>
                {
                    // �����ǰ���ڼ��ص���������Ϣ�е�������ͬ���򲻽����κβ���
                    // �����ǰ��δ�������⣬����������ͬһ��������Ȼ���أ���Ч��ˢ��
                    if (ViewModel.CurrentLoadingThreadId == m.Value)
                    {
                        return;
                    }
                    ViewModel.CurrentLoadingThreadId = m.Value;

                    // ȡ����һ�μ�������
                    _threadContentCancelTokenSource?.Cancel();
                    _threadContentCancelTokenSource?.Dispose();
                    _threadContentCancelTokenSource = new CancellationTokenSource();

                    try
                    {
                        // TODO ��ʾ���ض���
                        // TODO ��ʱʱ�������
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
                        // TODO ��ʾ��ʱ��Ϣ
                    }
                    catch (Exception ex)
                    {
                        // TODO ��ʾ������Ϣ

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
        /// ˢ�µ�ǰ���������б�
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
