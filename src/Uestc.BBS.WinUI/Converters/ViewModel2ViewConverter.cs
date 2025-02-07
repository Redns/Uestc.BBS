using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Data;
using Uestc.BBS.Core;
using Uestc.BBS.Mvvm.ViewModels;
using Uestc.BBS.WinUI.ViewModels;
using Uestc.BBS.WinUI.Views;

namespace Uestc.BBS.WinUI.Converters
{
    public partial class ViewModel2ViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is AuthViewModel)
                return ServiceExtension.Services.GetRequiredService<AuthPage>();
            if (value is HomeViewModel)
                return ServiceExtension.Services.GetRequiredService<HomePage>();
            if (value is SectionsViewModel)
                return ServiceExtension.Services.GetRequiredService<SectionsPage>();
            if (value is ServicesViewModel)
                return ServiceExtension.Services.GetRequiredService<ServicesPage>();
            if (value is MomentsViewModel)
                return ServiceExtension.Services.GetRequiredService<MomentsPage>();
            if (value is MessagesViewModel)
                return ServiceExtension.Services.GetRequiredService<MessagesPage>();
            if (value is SettingsViewModel)
                return ServiceExtension.Services.GetRequiredService<SettingsPage>();
            throw new ArgumentException("Value is not an ObservableObject type", nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
