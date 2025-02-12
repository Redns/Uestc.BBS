using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Uestc.BBS.Core;
using Uestc.BBS.Mvvm.Services;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Services
{
    public class NavigateService(IServiceProvider services) : INavigateService
    {
        private readonly IServiceProvider _services = services;

        public ObservableObject Navigate(MenuItemKey key) =>
            key switch
            {
                // Pages
                MenuItemKey.Home => _services.GetRequiredService<HomeViewModel>(),
                MenuItemKey.Sections => _services.GetRequiredService<SectionsViewModel>(),
                MenuItemKey.Services => _services.GetRequiredService<ServicesViewModel>(),
                MenuItemKey.Moments => _services.GetRequiredService<MomentsViewModel>(),
                MenuItemKey.Post => _services.GetRequiredService<PostViewModel>(),
                MenuItemKey.Messages => _services.GetRequiredService<MessagesViewModel>(),
                MenuItemKey.Settings => _services.GetRequiredService<SettingsViewModel>(),
                // Overlays
                MenuItemKey.MyFavorites => _services.GetRequiredService<MyFavoritesViewModel>(),
                MenuItemKey.MyPosts => _services.GetRequiredService<MyPostsViewModel>(),
                MenuItemKey.MyReplies => _services.GetRequiredService<MyRepliesViewModel>(),
                MenuItemKey.MyMarks => _services.GetRequiredService<MyMarksViewModel>(),
                MenuItemKey.TopicFilter => _services.GetRequiredService<TopicFilterViewModel>(),
                _ => throw new ArgumentException(
                    $"Navigate to {key} failed, unknown key",
                    nameof(key)
                ),
            };
    }
}
