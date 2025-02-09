using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Uestc.BBS.Mvvm.Services;
using Uestc.BBS.WinUI.ViewModels;
using Uestc.BBS.WinUI.Views;

namespace Uestc.BBS.WinUI.Services
{
    public class NavigateService(ServiceProvider services) : INavigateService
    {
        private readonly ServiceProvider _services = services;

        public ObservableObject Navigate(string view) =>
            view switch
            {
                nameof(AuthPage) => _services.GetRequiredService<AuthViewModel>(),
                nameof(HomePage) => _services.GetRequiredService<HomeViewModel>(),
                nameof(SettingsPage) => _services.GetRequiredService<SettingsViewModel>(),
                _
                    => throw new ArgumentException(
                        $"Navigate to {view} failed, unknown view",
                        nameof(view)
                    )
            };
    }
}
