using Microsoft.UI.Xaml.Controls;

namespace Uestc.BBS.WinUI.Views.ContentDialogs
{
    public sealed partial class AboutContentDialog : Page
    {
        /// <summary>
        /// �汾��
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// ��Ȩ��Ϣ
        /// </summary>
        public string CopyRight { get; set; } = string.Empty;

        /// <summary>
        /// Դ��ֿ��ַ
        /// </summary>
        public string SourceRepositoryUrl { get; set; } = "https://github.com/Redns/Uestc.BBS";

        public AboutContentDialog()
        {
            InitializeComponent();
        }
    }
}
