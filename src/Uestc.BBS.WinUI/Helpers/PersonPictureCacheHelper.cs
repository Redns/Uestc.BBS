using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.FileCache;

namespace Uestc.BBS.WinUI.Helpers
{
    public class PersonPictureCacheHelper
    {
        /// <summary>
        /// 文件缓存服务
        /// </summary>
        private static readonly IFileCache _fileCache =
            ServiceExtension.Services.GetRequiredService<IFileCache>();

        /// <summary>
        /// 对外暴露的 AttachedProperty
        /// </summary>
        public static readonly DependencyProperty ProfilePictureExProperty =
            DependencyProperty.RegisterAttached(
                "ProfilePictureEx",
                typeof(string),
                typeof(PersonPictureCacheHelper),
                new PropertyMetadata(null, OnProfilePictureExChanged)
            );

        public static string GetProfilePictureEx(PersonPicture obj) =>
            (string)obj.GetValue(ProfilePictureExProperty);

        public static void SetProfilePictureEx(PersonPicture obj, string profilePictureEx) =>
            obj.SetValue(ProfilePictureExProperty, profilePictureEx);

        private static async void OnProfilePictureExChanged(
            DependencyObject obj,
            DependencyPropertyChangedEventArgs args
        )
        {
            if (obj is not PersonPicture personPicture || args.NewValue is not string source)
            {
                return;
            }

            if (string.IsNullOrEmpty(source))
            {
                return;
            }

            personPicture.ProfilePicture = new BitmapImage(
                await _fileCache.GetFileUriAsync(new Uri(source))
            );
        }
    }
}
