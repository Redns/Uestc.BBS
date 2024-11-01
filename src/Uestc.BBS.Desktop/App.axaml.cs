using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using System;
using Uestc.BBS.Desktop.ViewModels;
using Uestc.BBS.ViewModels;
using Uestc.BBS.Views;

namespace Uestc.BBS.Desktop
{
    public partial class App : Application
    {
        /// <summary>
        /// 应用服务
        /// </summary>
        public static IServiceProvider Services { get; private set; }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            Services = ConfigureServices();

            // 移除 Avalonia 数据验证否则数据验证将在 Avalonia 和 CommunityToolkit 中重复
            BindingPlugins.DataValidators.RemoveAt(0);
            //  获取用户登陆状态
            var isUserAuthorized = false;
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Line below is needed to remove Avalonia data validation.
                // Without this line you will get duplicate validations from both Avalonia and CT
                BindingPlugins.DataValidators.RemoveAt(0);
                // 根据当前登陆状态打开窗口
                if (isUserAuthorized)
                {
                    desktop.MainWindow = new MainWindow();
                }
                else
                {
                    desktop.MainWindow = new AuthWindow();
                }
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                if (isUserAuthorized)
                {
                    singleViewPlatform.MainView = new MainView();
                }
                else
                {
                    singleViewPlatform.MainView = new AuthView();
                }
            }

            base.OnFrameworkInitializationCompleted();
        }

        /// <summary>
        /// 依赖服务注入
        /// </summary>
        /// <returns></returns>
        private ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddTransient<MainViewModel>();
            services.AddTransient<AuthViewModel>();
            services.AddTransient<HomeViewModel>();

            return services.BuildServiceProvider();
        }
    }
}