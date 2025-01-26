using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core;
using Uestc.BBS.WinUI.Pages;

namespace Uestc.BBS.WinUI.Services.NavigateService
{
    public class NavigateService : INavigateService
    {
        public Page Navigate(string uri)
        {
            return $"{uri}Page" switch
            {
                nameof(HomePage) => ServiceExtension.Services.GetRequiredService<HomePage>(),
                nameof(SectionsPage) =>
                    ServiceExtension.Services.GetRequiredService<SectionsPage>(),
                nameof(ServicesPage) =>
                    ServiceExtension.Services.GetRequiredService<ServicesPage>(),
                nameof(MomentsPage) => ServiceExtension.Services.GetRequiredService<MomentsPage>(),
                nameof(PostPage) => ServiceExtension.Services.GetRequiredService<PostPage>(),
                nameof(MessagesPage) =>
                    ServiceExtension.Services.GetRequiredService<MessagesPage>(),
                nameof(SettingsPage) =>
                    ServiceExtension.Services.GetRequiredService<SettingsPage>(),
                _ => throw new ArgumentException(
                    $"Navigate failed, unknown page {uri}",
                    nameof(uri)
                ),
            };
        }
    }
}
