using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core.Services.Api.Forum;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Views
{
    public sealed partial class HomePage : Page
    {
        private HomeViewModel ViewModel { get; init; }

        private TopicContent[] TopicContent { get; set; } =
            [
                new TopicContent
                {
                    Information =
                        "��ϲ������У԰���� ��ǣ�ֵ��Ҹ� [mobcent_phiz=https://bbs.uestc.edu.cn/static/image/smiley/alu/52.gif][mobcent_phiz=https://bbs.uestc.edu.cn/static/image/smiley/alu/52.gif]\r\n��͵͵��¼һ�� �������ð���Ҿ�ɾŶ��[mobcent_phiz=https://bbs.uestc.edu.cn/static/image/smiley/yellowface/(26).gif]",
                    Type = TopicContenType.Text
                },
                new TopicContent
                {
                    Information =
                        "https://bbs.uestc.edu.cn/thumb/data/attachment/forum/202502/19/202746likejj0ra17977r9.png?variant=original",
                    Type = TopicContenType.Image,
                    OriginalInformation =
                        "https://bbs.uestc.edu.cn/thumb/data/attachment/forum/202502/19/202746likejj0ra17977r9.png?variant=original",
                    Aid = 2519102
                }
            ];

        public HomePage(HomeViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
        }
    }
}
