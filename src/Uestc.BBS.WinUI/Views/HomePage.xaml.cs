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
                    Information = "������������ԡ�",
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
                        "���Ų��ö�þ��ܸγ���ƪ�ܲ�����ܽᣬȻ���ĥ�󹤣��ڼ����⼸����Ϣ��������ʮ�ֻ��ң��͵����ˡ����һд��������д���꣬���д���꣬˵��д�ܲ����м�Ҳ�����˲���С���£�����Ӧ����ɣ���������Ҳ����flag�� [mobcent_phiz=https://bbs.uestc.edu.cn/static/image/smiley/alu/33.gif]\r\n\r\n������ƪ��о�������Ŀǰ��ʱ����ˣ��ҷ���������Ĳ���д��з���Ķ������ܸо��ʲ����⣬����ÿ����ȥ����ϸ�ڵ�ʱ��������ƽ����������������λ���£��������� [mobcent_phiz=https://bbs.uestc.edu.cn/static/image/smiley/alu/42.gif]",
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
