using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Models;

namespace Uestc.BBS.WinUI.Views.ContentDialogs
{
    public sealed partial class AboutContentDialog : UserControl
    {
        private static readonly DependencyProperty AppmanifestProperty =
            DependencyProperty.Register(
                nameof(Appmanifest),
                typeof(Appmanifest),
                typeof(AboutContentDialog),
                new PropertyMetadata(new())
            );

        public Appmanifest Appmanifest
        {
            get => (Appmanifest)GetValue(AppmanifestProperty);
            set => SetValue(AppmanifestProperty, value);
        }

        public AboutContentDialog()
        {
            InitializeComponent();
        }

        [RelayCommand]
        private void ContactDeveloper()
        {
            if (string.IsNullOrEmpty(Appmanifest.ContactEmail))
            {
                return;
            }

            OperatingSystemHelper.OpenWebsite("mailto:" + Appmanifest.ContactEmail);
        }
    }
}
