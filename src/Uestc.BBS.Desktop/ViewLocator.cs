using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;
using Uestc.BBS.Desktop.ViewModels;
using Uestc.BBS.Views;

namespace Uestc.BBS.Desktop
{
    public class ViewLocator : IDataTemplate
    {
        public Control? Build(object? data)
        {
            if (data is AuthViewModel) return new AuthView();
            if (data is HomeViewModel) return new HomeView();
            throw new NotImplementedException();
        }

        public bool Match(object? data)
        {
            return data is ViewModelBase;
        }
    }
}