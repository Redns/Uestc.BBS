using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Data;
using Uestc.BBS.Core;
using Uestc.BBS.Mvvm.ViewModels;
using Uestc.BBS.WinUI.ViewModels;
using Uestc.BBS.WinUI.Views;
using Uestc.BBS.WinUI.Views.Overlays;

namespace Uestc.BBS.WinUI.Converters
{
    public partial class ViewModel2ViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is AuthViewModel)
                return ServiceExtension.Services.GetRequiredService<AuthPage>();
            if (value is MainViewModel)
                return ServiceExtension.Services.GetRequiredService<MainPage>();
            if (value is HomeViewModel)
                return ServiceExtension.Services.GetRequiredService<HomePage>();
            if (value is SectionsViewModel)
                return ServiceExtension.Services.GetRequiredService<SectionsPage>();
            if (value is ServicesViewModel)
                return ServiceExtension.Services.GetRequiredService<ServicesPage>();
            if (value is MomentsViewModelBase)
                return ServiceExtension.Services.GetRequiredService<MomentsPage>();
            if (value is PostViewModel)
                return ServiceExtension.Services.GetRequiredService<PostPage>();
            if (value is MessagesViewModel)
                return ServiceExtension.Services.GetRequiredService<MessagesPage>();
            if (value is SettingsViewModel)
                return ServiceExtension.Services.GetRequiredService<SettingsPage>();
            if (value is MyFavoritesViewModel)
                return ServiceExtension.Services.GetRequiredService<MyFavoritesOverlay>();
            if (value is MyPostsViewModel)
                return ServiceExtension.Services.GetRequiredService<MyPostsOverlay>();
            if (value is MyRepliesViewModel)
                return ServiceExtension.Services.GetRequiredService<MyRepliesOverlay>();
            if (value is MyMarksViewModel)
                return ServiceExtension.Services.GetRequiredService<MyMarksOverlay>();
            if (value is TopicFilterViewModel)
                return ServiceExtension.Services.GetRequiredService<TopicFilterOverlay>();
            throw new ArgumentException(
                "Convert view model to view failed, value is not an ObservableObject type",
                nameof(value)
            );
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
