using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Mvvm.Messages;
using Uestc.BBS.Sdk.Services.Thread;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Views
{
    public sealed partial class HomePage : Page
    {
        private HomeViewModel ViewModel { get; init; }

        private readonly IThreadContentService _threadContentService;

        /// <summary>
        /// 调度任务队列
        /// </summary>
        private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        //private RichTextContent[] TopicContents { get; init; } =
        //    [
        //        new RichTextContent
        //        {
        //            Information = "【心理年龄测试】",
        //            Type = TopicContenType.Text,
        //        },
        //        new RichTextContent
        //        {
        //            Information =
        //                "抖音刷到一个晒自己《机甲兽神》玩具的视频，不知道还有多少pu知道这个动画片[mobcent_phiz=https://bbs.uestcer.org/static/image/smiley/alu/31.gif]，想到动画片里那么燃的战车对撞，到现实中正版玩具都只能龟速向前蠕动，这落差还是挺大的[mobcent_phiz=https://bbs.uestcer.org/static/image/smiley/alu/31.gif]\r\n\r\n我自己的话印象中是买了一辆正版的《战龙四驱》的主角车，不记得叫啥了，结果它在玩具店老板的轨道里跑不过当时旁边小朋友的盗版车[mobcent_phiz=https://bbs.uestcer.org/static/image/smiley/alu/33.gif]小小的lz当时心灵受到了一万点暴击[mobcent_phiz=https://bbs.uestcer.org/static/image/smiley/alu/58.gif]",
        //            Type = TopicContenType.Text,
        //        },
        //        new RichTextContent
        //        {
        //            Information = "https://www.arealme.com/mental/cn/?_refluxos=a10",
        //            Type = TopicContenType.InlineLink,
        //            Url = "https://www.arealme.com/mental/cn/?_refluxos=a10",
        //            OriginalInformation =
        //                "https://bbs.uestcer.org/thumb/data/attachment/forum/202502/18/234054tcja8jxnfj5eyf1k.png?variant=original",
        //        },
        //        //new RichTextContent
        //        //{
        //        //    Information =
        //        //        "https://bbs.uestcer.org/thumb/data/attachment/forum/202502/18/234054tcja8jxnfj5eyf1k.png?variant=original",
        //        //    Type = TopicContenType.Image,
        //        //    Url =
        //        //        "https://bbs.uestcer.org/data/attachment/forum/202502/18/234054tcja8jxnfj5eyf1k.png",
        //        //    Aid = 2518777,
        //        //},
        //        new RichTextContent
        //        {
        //            Information =
        //                "想着不用多久就能肝出这篇跑步年度总结，然后就磨洋工，在加上这几天作息，心绪都十分混乱，就耽搁了。结果一写起来发现写不完，真的写不完，说是写跑步，中间也穿插了不少小故事，明天应该完成，呜呜呜再也不立flag了 [mobcent_phiz=https://bbs.uestcer.org/static/image/smiley/alu/33.gif]\r\n\r\n至于那篇情感经历贴，目前暂时搁笔了，我发现我是真的不会写情感方面的东西，总感觉词不达意，而且每当我去回忆细节的时候心绪总平静不下来，哎，各位见谅，能力有限 [mobcent_phiz=https://bbs.uestcer.org/static/image/smiley/alu/42.gif]",
        //            Type = TopicContenType.Text,
        //        },
        //        //new RichTextContent
        //        //{
        //        //    Information =
        //        //        "https://bbs.uestcer.org/thumb/data/attachment/forum/202502/18/1feff0_6vvfn665v5iqsybsmi90xk8i.png?variant=original",
        //        //    Type = TopicContenType.Image,
        //        //    OriginalInformation =
        //        //        "https://bbs.uestcer.org/thumb/data/attachment/forum/202502/18/1feff0_6vvfn665v5iqsybsmi90xk8i.png?variant=original",
        //        //    Aid = 2518783,
        //        //},
        //    ];

        public HomePage(HomeViewModel viewModel, IThreadContentService threadContentService)
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

            _threadContentService = threadContentService;

            // 注册主题选择消息
            StrongReferenceMessenger.Default.Register<ThreadChangedMessage>(
                this,
                async (_, m) =>
                    await _threadContentService
                        .GetThreadContentAsync(m.Value)
                        .ContinueWith(t =>
                            _dispatcherQueue.EnqueueAsync(() => viewModel.CurrentThread = t.Result)
                        )
            );
        }

        ~HomePage()
        {
            StrongReferenceMessenger.Default.Unregister<ThreadChangedMessage>(this);
        }
    }
}
