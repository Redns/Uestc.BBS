using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Uestc.BBS.Core.Services.Forum;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Views
{
    public sealed partial class HomePage : Page
    {
        private BoardTabItemModel? _lastBoardTabItem;

        private HomeViewModel ViewModel { get; init; }

        private RichTextContent[] TopicContents { get; set; } =
            [
                new RichTextContent
                {
                    Information = "【心理年龄测试】",
                    Type = TopicContenType.Text,
                },
                new RichTextContent
                {
                    Information = "https://www.arealme.com/mental/cn/?_refluxos=a10",
                    Type = TopicContenType.InlineLink,
                    Url = "https://www.arealme.com/mental/cn/?_refluxos=a10",
                    OriginalInformation =
                        "https://bbs.uestc.edu.cn/thumb/data/attachment/forum/202502/18/234054tcja8jxnfj5eyf1k.png?variant=original",
                },
                new RichTextContent
                {
                    Information =
                        "https://bbs.uestc.edu.cn/thumb/data/attachment/forum/202502/18/234054tcja8jxnfj5eyf1k.png?variant=original",
                    Type = TopicContenType.Image,
                    Url =
                        "https://bbs.uestc.edu.cn/data/attachment/forum/202502/18/234054tcja8jxnfj5eyf1k.png",
                    Aid = 2518777,
                },
                new RichTextContent
                {
                    Information =
                        "想着不用多久就能肝出这篇跑步年度总结，然后就磨洋工，在加上这几天作息，心绪都十分混乱，就耽搁了。结果一写起来发现写不完，真的写不完，说是写跑步，中间也穿插了不少小故事，明天应该完成，呜呜呜再也不立flag了 [mobcent_phiz=https://bbs.uestc.edu.cn/static/image/smiley/alu/33.gif]\r\n\r\n至于那篇情感经历贴，目前暂时搁笔了，我发现我是真的不会写情感方面的东西，总感觉词不达意，而且每当我去回忆细节的时候心绪总平静不下来，哎，各位见谅，能力有限 [mobcent_phiz=https://bbs.uestc.edu.cn/static/image/smiley/alu/42.gif]",
                    Type = TopicContenType.Text,
                },
                new RichTextContent
                {
                    Information =
                        "https://bbs.uestc.edu.cn/thumb/data/attachment/forum/202502/18/1feff0_6vvfn665v5iqsybsmi90xk8i.png?variant=original",
                    Type = TopicContenType.Image,
                    OriginalInformation =
                        "https://bbs.uestc.edu.cn/thumb/data/attachment/forum/202502/18/1feff0_6vvfn665v5iqsybsmi90xk8i.png?variant=original",
                    Aid = 2518783,
                },
            ];

        public HomePage(HomeViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;

            ViewModel.PropertyChanging += (_, e) =>
            {
                if (e.PropertyName == nameof(ViewModel.CurrentBoardTabItemModel))
                {
                    _lastBoardTabItem = ViewModel.CurrentBoardTabItemModel;
                }
            };

            ViewModel.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName != nameof(ViewModel.CurrentBoardTabItemModel))
                {
                    return;
                }

                var isRight2Left =
                    ViewModel.CurrentBoardTabItemModel == null
                    || _lastBoardTabItem == null
                    || ViewModel.AppSettingModel.Appearance.BoardTab.Items.IndexOf(
                        ViewModel.CurrentBoardTabItemModel
                    )
                        > ViewModel.AppSettingModel.Appearance.BoardTab.Items.IndexOf(
                            _lastBoardTabItem
                        );

                BoardSwitchStoryboard
                    .Children[0]
                    .SetValue(DoubleAnimation.FromProperty, isRight2Left ? 100 : -100);
                BoardSwitchStoryboard.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
                BoardSwitchStoryboard.Begin();
            };
        }
    }
}
