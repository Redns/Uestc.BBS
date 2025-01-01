using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.Api.Forum;
using Uestc.BBS.Desktop.Converters;
using Uestc.BBS.Desktop.Models;

namespace Uestc.BBS.Desktop.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly AppSetting _appSetting;

        private readonly ITopicService _topicService;

        /// <summary>
        /// 集合非空转换器
        /// </summary>
        public static readonly ObservableCollectionIsNotEmptyConverter<TopicOverview> TopicOverviewObservableCollectionIsNotEmptyConverter =
            new();

        /// <summary>
        /// 当前选中的 Tab 栏
        /// </summary>
        [ObservableProperty]
        private BoardTabItemModel? _currentBoardTabItemModel;

        /// <summary>
        /// 版块 Tab 栏集合
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<BoardTabItemModel> _boardTabItems;

        public HomeViewModel(AppSetting appSetting, ITopicService topicService)
        {
            _appSetting = appSetting;
            _topicService = topicService;
            _boardTabItems = new ObservableCollection<BoardTabItemModel>(
                appSetting.Apperance.BoardTabItems.Select(item => new BoardTabItemModel
                {
                    Name = item.Name,
                    Board = item.Board,
                    SortType = item.SortType,
                    PageSize = item.PageSize,
                    RequirePreviewSources = item.RequirePreviewSources,
                })
            );

            // 加载板块帖子
            Task.WhenAll(
                _boardTabItems.Select(tabItem =>
                    Task.Run(() =>
                    {
                        tabItem.IsLoading = true;
                        tabItem.Topics = _topicService
                            .GetTopicsAsync(
                                pageSize: tabItem.PageSize,
                                boardId: tabItem.Board,
                                sortby: tabItem.SortType,
                                getPreviewSources: tabItem.RequirePreviewSources
                            )
                            .ContinueWith(t =>
                            {
                                tabItem.IsLoading = false;
                                return new ObservableCollection<TopicOverview>(
                                    t.Result?.List.Length > 0 ? t.Result.List : []
                                );
                            });
                    })
                )
            );
        }

        /// <summary>
        /// 滚动加载帖子
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [RelayCommand]
        private async Task LoadTopicsAsync(ScrollChangedEventArgs args)
        {
            if (args.Source is not ScrollViewer scrollViewer)
            {
                return;
            }

            // Offset.Length：偏移量
            // Extent.Height：可滚动范围
            // DesiredSize.Height：窗体高度
            // 剩余可滚动内容高度小于窗体高度的两倍时加载数据
            var remainContentHeight = scrollViewer.Extent.Height - scrollViewer.Offset.Length;
            if (remainContentHeight > scrollViewer.DesiredSize.Height * 2)
            {
                return;
            }

            CurrentBoardTabItemModel!.IsLoading = true;

            // 加载帖子
            var currentTopics = await CurrentBoardTabItemModel.Topics;
            foreach (
                var topic in await _topicService
                    .GetTopicsAsync(
                        page: (uint)CurrentBoardTabItemModel.Topics.Result.Count
                            / CurrentBoardTabItemModel.PageSize
                            + 1,
                        pageSize: CurrentBoardTabItemModel.PageSize,
                        boardId: CurrentBoardTabItemModel.Board,
                        sortby: CurrentBoardTabItemModel.SortType,
                        getPreviewSources: CurrentBoardTabItemModel.RequirePreviewSources
                    )
                    .ContinueWith(t => new ObservableCollection<TopicOverview>(
                        t.Result?.List ?? []
                    ))
            )
            {
                currentTopics.Add(topic);
            }

            CurrentBoardTabItemModel.IsLoading = false;
        }
    }
}
