using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Uestc.BBS.Core;
using Uestc.BBS.WinUI.ViewModels;
using Windows.Foundation;

namespace Uestc.BBS.WinUI.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel { get; init; } =
            ServiceExtension.Services.GetRequiredService<MainViewModel>();

        public MainPage()
        {
            InitializeComponent();

            ViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName != nameof(ViewModel.CurrentMenuKey))
                {
                    return;
                }

                var transition = new TranslateTransform();

                ViewModel.CurrentMenuContent.RenderTransform = transition;
                ViewModel.CurrentMenuContent.RenderTransformOrigin = new Point(0.5, 0.5);

                var storyboard = (Storyboard)Application.Current.Resources["NavigateStoryboard"];

                storyboard.Stop();
                Storyboard.SetTargetName(storyboard.Children[0], "Translation");
                Storyboard.SetTarget(storyboard, transition);
                storyboard.Begin();
            };
        }

        /// <summary>
        /// 展开个人中心弹窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenPersonalCenterFlyout(object sender, PointerRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }

        /// <summary>
        /// 清除顶部菜单的选中状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ClearTopMenuSelection(
            NavigationView sender,
            NavigationViewItemInvokedEventArgs args
        ) => TopMenuBar.SelectedIndex = -1;
    }
}
