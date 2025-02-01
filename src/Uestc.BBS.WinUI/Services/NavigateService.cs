using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.NavigateService;
using Uestc.BBS.Core.ViewModels;
using Uestc.BBS.WinUI.Views;

namespace Uestc.BBS.WinUI.Services
{
    public class NavigateService : INavigateService
    {
        public ObservableObject Navigate(string uri)
        {
            return $"{uri}Page" switch
            {
                nameof(HomePage) => ServiceExtension.Services.GetRequiredService<HomeViewModel>(),
                nameof(SectionsPage) =>
                    ServiceExtension.Services.GetRequiredService<SectionsViewModel>(),
                nameof(ServicesPage) =>
                    ServiceExtension.Services.GetRequiredService<ServicesViewModel>(),
                nameof(MomentsPage) => ServiceExtension.Services.GetRequiredService<MomentsViewModel>(),
                nameof(PostPage) => ServiceExtension.Services.GetRequiredService<PostViewModel>(),
                nameof(MessagesPage) =>
                    ServiceExtension.Services.GetRequiredService<MessagesViewModel>(),
                nameof(SettingsPage) =>
                    ServiceExtension.Services.GetRequiredService<SettingsViewModel>(),
                _ => throw new ArgumentException(
                    $"Navigate failed, unknown page {uri}",
                    nameof(uri)
                ),
            };
        }
    }
}
