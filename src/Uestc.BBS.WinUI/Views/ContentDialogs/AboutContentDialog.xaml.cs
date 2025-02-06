using Microsoft.UI.Xaml.Controls;

namespace Uestc.BBS.WinUI.Views.ContentDialogs
{
    public sealed partial class AboutContentDialog : Page
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// 版权信息
        /// </summary>
        public string CopyRight { get; set; } = string.Empty;

        public AboutContentDialog()
        {
            InitializeComponent();
        }
    }
}
