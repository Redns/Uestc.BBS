using Avalonia;
using Avalonia.Controls;
using System;

namespace Uestc.BBS.Desktop.Helpers
{
    public static class AppHelper
    {
        private static string? _appVersion;
        public static string AppVersion
        {
            get
            {
                if (string.IsNullOrEmpty(_appVersion))
                {
                    _appVersion = (string)Application.Current?.FindResource("AppVersion")!;
                }
                return _appVersion;
            }
        }

        private static DateTime _originalDatetime;
        public static DateTime OriginalDatetime
        {
            get
            {
                if (_originalDatetime == default)
                {
                    _originalDatetime = (DateTime)Application.Current?.FindResource("OriginalDatetime")!;
                }
                return _originalDatetime;
            }
        }
    }
}
