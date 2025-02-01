using System;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core.ViewModels;
using Uestc.BBS.WinUI.Helpers;
using Windows.UI;

namespace Uestc.BBS.WinUI.Views
{
    public sealed partial class AuthWindow : Window
    {
        private readonly AuthViewModel _viewModel;

        public AuthWindow(AuthViewModel viewModel)
        {
            Init();
            InitializeComponent();

            _viewModel = viewModel;
        }

        private void Init()
        {
            var appWindow = this.GetCurrentWindow();

            // �Զ��������
            SetTitleBar(AppTitleBar);
            // ������չ��������
            ExtendsContentIntoTitleBar = true;
            // ���ô��ڴ�С
            appWindow.Resize(new Windows.Graphics.SizeInt32(480, 400));
            appWindow.TitleBar.BackgroundColor = Color.FromArgb(0, 0, 0, 0);
            appWindow.TitleBar.IconShowOptions = Microsoft.UI.Windowing.IconShowOptions.HideIconAndSystemMenu;
        }

        private void Username_AutoSuggestBox_TextChanged(
            AutoSuggestBox sender,
            AutoSuggestBoxTextChangedEventArgs args
        )
        {
            if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput)
            {
                return;
            }

            sender.ItemsSource = _viewModel
                .Users.Where(u => u.Name.Contains(sender.Text, StringComparison.OrdinalIgnoreCase))
                //.Select(u => u.Name)
                .ToList();
        }
    }
}
