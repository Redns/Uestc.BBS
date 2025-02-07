using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.Api.Forum;
using Uestc.BBS.Mvvm.Models;

namespace Uestc.BBS.Mvvm.ViewModels
{
    public abstract partial class HomeViewModelBase : ObservableObject
    {
        private readonly AppSetting _appSetting;

        private readonly ITopicService _topicService;

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
        public partial string MarkdownContent { get; set; } =
            "生物钟彻底乱了，晚上八点迷迷糊糊睡着现在就醒了，刷到生动民主实践的帖子，对不起我有罪我脑海里第一时间出现的居然是这个\r\n\r\n![](https://bbs.uestc.edu.cn/thumb/data/attachment/forum/202501/10/033148ovvv227vd2fif2c4.png)\r\n\r\n当然我说的是😓\r\n旧版河畔右上角会有每日好句，感觉挺有意思想找接口没找到，所以直接暴力获取首页 html 然后解析，代码比较简单\r\n\r\n```js\r\nexport default {\r\n  async fetch(request, env, ctx) {\r\n    // 获取首页内容\r\n    const html = await (await fetch('https://bbs.uestc.edu.cn/forum.php?mobile=no')).text();\r\n    // 使用正则表达式匹配字符串\r\n    const regex = /<div class=\"vanfon_geyan\">.*?<span[^>]*>(.*?)<\\/span>.*?<\\/div>/s;\r\n    const match = regex.exec(html);\r\n    if(match)\r\n    {\r\n      return new Response(match[1]);\r\n    }\r\n    return new Response('获取失败~');\r\n  },\r\n};```\r\n";

        public HomeViewModelBase(AppSetting appSetting, ITopicService topicService)
        {
            _appSetting = appSetting;
            _topicService = topicService;
            BoardTabItems =
            [
                .. appSetting.Apperance.BoardTabItems.Select(item => new BoardTabItemModel
                {
                    Name = item.Name,
                    Route = item.Route,
                    Board = item.Board,
                    SortType = item.SortType,
                    PageSize = item.PageSize,
                    RequirePreviewSources = item.RequirePreviewSources,
                    ModuleId = item.ModuleId,
                }),
            ];

            // 加载板块帖子
            Task.WhenAll(
                BoardTabItems.Select(tabItem =>
                    DispatcherAsync(async () =>
                    {
                        tabItem.IsLoading = true;

                        var topics = await _topicService.GetTopicsAsync(
                            route: tabItem.Route,
                            pageSize: tabItem.PageSize,
                            boardId: tabItem.Board,
                            sortby: tabItem.SortType,
                            moduleId: tabItem.ModuleId,
                            getPreviewSources: tabItem.RequirePreviewSources
                        );

                        if (topics?.List.Length > 0)
                        {
                            foreach (var topic in topics.List)
                            {
                                tabItem.Topics.Add(topic);
                            }
                        }

                        tabItem.IsLoading = false;
                    })
                )
            );
        }

        public abstract Task DispatcherAsync(Action action);
    }
}
