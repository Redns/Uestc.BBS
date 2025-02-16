using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core;
using Uestc.BBS.Mvvm.Services;
using Uestc.BBS.WinUI.Views;
using Uestc.BBS.WinUI.Views.Overlays;

namespace Uestc.BBS.WinUI.Services
{
    public class NavigateService(IServiceProvider services) : INavigateService<Page>
    {
        private readonly IServiceProvider _services = services;

        public Page Navigate(MenuItemKey key) =>
            key switch
            {
                // Pages
                MenuItemKey.Home => _services.GetRequiredService<HomePage>(),
                MenuItemKey.Sections => _services.GetRequiredService<SectionsPage>(),
                MenuItemKey.Services => _services.GetRequiredService<ServicesPage>(),
                MenuItemKey.Moments => _services.GetRequiredService<MomentsPage>(),
                MenuItemKey.Post => _services.GetRequiredService<PostPage>(),
                MenuItemKey.Messages => _services.GetRequiredService<MessagesPage>(),
                MenuItemKey.Settings => _services.GetRequiredService<SettingsPage>(),
                // TopMenuBar Overlays
                MenuItemKey.MyFavorites => _services.GetRequiredService<MyFavoritesOverlay>(),
                MenuItemKey.MyPosts => _services.GetRequiredService<MyPostsOverlay>(),
                MenuItemKey.MyReplies => _services.GetRequiredService<MyRepliesOverlay>(),
                MenuItemKey.MyMarks => _services.GetRequiredService<MyMarksOverlay>(),
                MenuItemKey.TopicFilter => _services.GetRequiredService<BrowseSettingsOverlay>(),
                // Settings Overlays
                MenuItemKey.ApperanceSettings =>
                    _services.GetRequiredService<ApperanceSettingsOverlay>(),
                MenuItemKey.BrowseSettings => _services.GetRequiredService<BrowseSettingsOverlay>(),
                MenuItemKey.AccountSettings =>
                    _services.GetRequiredService<AccountSettingsOverlay>(),
                MenuItemKey.NotificationSettings =>
                    _services.GetRequiredService<NotificationSettingsOverlay>(),
                MenuItemKey.StorageSettings =>
                    _services.GetRequiredService<StorageSettingsOverlay>(),
                MenuItemKey.ServicesSettings =>
                    _services.GetRequiredService<ServicesSettingsOverlay>(),
                _ => throw new ArgumentException(
                    $"Navigate to {key} failed, unknown key",
                    nameof(key)
                ),
            };
    }
}
