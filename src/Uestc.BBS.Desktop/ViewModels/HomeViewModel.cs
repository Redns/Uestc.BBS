using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Uestc.BBS.Core.Models;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Sdk.Services.Thread.ThreadList;

namespace Uestc.BBS.Desktop.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly AppSetting _appSetting;

        private readonly IThreadListService _threadService;

        /// <summary>
        /// 当前选中的 Tab 栏
        /// </summary>
        [ObservableProperty]
        public partial BoardTabItemModel? CurrentBoardTabItemModel { get; set; }

        /// <summary>
        /// 版块 Tab 栏集合
        /// </summary>
        [ObservableProperty]
        public partial ObservableCollection<BoardTabItemModel> BoardTabItems { get; set; }

        [ObservableProperty]
        private string _markdownContent =
            "生物钟彻底乱了，晚上八点迷迷糊糊睡着现在就醒了，刷到生动民主实践的帖子，对不起我有罪我脑海里第一时间出现的居然是这个\r\n\r\n![](https://bbs.uestc.edu.cn/thumb/data/attachment/forum/202501/10/033148ovvv227vd2fif2c4.png)\r\n\r\n当然我说的是😓\r\n旧版河畔右上角会有每日好句，感觉挺有意思想找接口没找到，所以直接暴力获取首页 html 然后解析，代码比较简单\r\n\r\n```js\r\nexport default {\r\n  async fetch(request, env, ctx) {\r\n    // 获取首页内容\r\n    const html = await (await fetch('https://bbs.uestc.edu.cn/forum.php?mobile=no')).text();\r\n    // 使用正则表达式匹配字符串\r\n    const regex = /<div class=\"vanfon_geyan\">.*?<span[^>]*>(.*?)<\\/span>.*?<\\/div>/s;\r\n    const match = regex.exec(html);\r\n    if(match)\r\n    {\r\n      return new Response(match[1]);\r\n    }\r\n    return new Response('获取失败~');\r\n  },\r\n};```\r\n";

        public HomeViewModel(AppSetting appSetting, IThreadListService topicService)
        {
            _appSetting = appSetting;
            _threadService = topicService;
            BoardTabItems =
            [
                .. appSetting.Appearance.BoardTab.Items.Select(item => new BoardTabItemModel(item)),
            ];

            // 加载板块帖子
            Task.WhenAll(
                BoardTabItems.Select(tabItem =>
                    Task.Run(async () =>
                    {
                        // tabItem.IsLoading = true;

                        var topics = await _threadService.GetThreadListAsync(
                            route: tabItem.Route,
                            pageSize: 30,
                            boardId: tabItem.Board,
                            sortby: tabItem.SortType,
                            moduleId: tabItem.ModuleId,
                            getPreviewSources: tabItem.RequirePreviewSources
                        );

                        if (topics?.List.Length > 0)
                        {
                            tabItem.Topics = [.. topics.List];
                        }

                        // tabItem.IsLoading = false;
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
        private async Task ScrollToLoadTopicsAsync(ScrollChangedEventArgs args)
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

            await LoadTopicsAsync();
        }

        private async Task LoadTopicsAsync()
        {
            // CurrentBoardTabItemModel!.IsLoading = true;

            // 加载帖子
            var currentTopics = CurrentBoardTabItemModel.Topics;
            foreach (
                var topic in await _threadService
                    .GetTopicsAsync(
                        page: (uint)CurrentBoardTabItemModel.Topics.Count
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
                if (currentTopics.Any(t => t.TopicId == topic.TopicId))
                {
                    continue;
                }
                currentTopics.Add(topic);
            }

            // CurrentBoardTabItemModel.IsLoading = false;
        }

        /// <summary>
        /// 刷新当前板块帖子
        /// </summary>
        [RelayCommand]
        private async Task RefreshCurrentBoardTopicsAsync(BoardTabItemModel model)
        {
            if (model.Equals(CurrentBoardTabItemModel) != true)
            {
                return;
            }

            //if (CurrentBoardTabItemModel.IsLoading)
            //{
            //    return;
            //}

            CurrentBoardTabItemModel.Topics.Clear();
            await LoadTopicsAsync();
        }
    }
}
