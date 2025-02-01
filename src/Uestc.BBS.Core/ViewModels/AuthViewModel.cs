using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Uestc.BBS.Core.ViewModels
{
    public partial class AuthViewModel : ObservableObject
    {
        private readonly AppSetting _appSetting;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(UsernameMessage))]
        public partial string Username { get; set; } = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PasswordMessage))]
        public partial string Password { get; set; } = string.Empty;

        [ObservableProperty]
        public partial bool AutoLogin { get; set; } = true;

        [ObservableProperty]
        public partial bool RememberPassword { get; set; } = true;

        public string UsernameMessage => Username == string.Empty ? "请输入用户名" : string.Empty;

        public string PasswordMessage => Password == string.Empty ? "请输入密码" : string.Empty;

        public AuthViewModel(AppSetting appSetting)
        {
            _appSetting = appSetting;
        }

        [RelayCommand]
        private void Login() { }
    }
}
