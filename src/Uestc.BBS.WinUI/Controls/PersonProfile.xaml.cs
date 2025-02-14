using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Uestc.BBS.Core.Helpers;

namespace Uestc.BBS.WinUI.Controls
{
    public sealed partial class PersonProfile : UserControl
    {
        /// <summary>
        /// 头像
        /// </summary>
        private static readonly DependencyProperty AvatarProperty = DependencyProperty.Register(
            nameof(Avatar),
            typeof(ImageSource),
            typeof(PersonProfile),
            new PropertyMetadata(default(ImageSource))
        );

        public ImageSource Avatar
        {
            get => (ImageSource)GetValue(AvatarProperty);
            set => SetValue(AvatarProperty, value);
        }

        /// <summary>
        /// 用户等级
        /// </summary>
        private static readonly DependencyProperty LevelProperty = DependencyProperty.Register(
            nameof(Level),
            typeof(uint),
            typeof(PersonProfile),
            new PropertyMetadata(default(uint))
        );

        public uint Level
        {
            get => (uint)GetValue(LevelProperty);
            set => SetValue(LevelProperty, value);
        }

        /// <summary>
        /// 用户组
        /// </summary>
        private static readonly DependencyProperty GroupProperty = DependencyProperty.Register(
            nameof(Group),
            typeof(string),
            typeof(PersonProfile),
            new PropertyMetadata(StringHelper.WhiteSpace)
        );

        public string Group
        {
            get => (string)GetValue(GroupProperty);
            set => SetValue(GroupProperty, value);
        }

        /// <summary>
        /// 用户名
        /// </summary>
        private static readonly DependencyProperty UserNameProperty = DependencyProperty.Register(
            nameof(UserName),
            typeof(string),
            typeof(PersonProfile),
            new PropertyMetadata(StringHelper.WhiteSpace)
        );

        public string UserName
        {
            get => (string)GetValue(UserNameProperty);
            set => SetValue(UserNameProperty, value);
        }

        /// <summary>
        /// 签名
        /// </summary>
        private static readonly DependencyProperty SignatureProperty = DependencyProperty.Register(
            nameof(Signature),
            typeof(string),
            typeof(PersonProfile),
            new PropertyMetadata(StringHelper.WhiteSpace)
        );

        public string Signature
        {
            get => (string)GetValue(SignatureProperty);
            set => SetValue(SignatureProperty, value);
        }

        public PersonProfile()
        {
            InitializeComponent();
        }
    }
}
