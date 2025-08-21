using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.FileCache;
using Uestc.BBS.Core.Services.System;

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
        /// 日志服务
        /// </summary>
        private static readonly ILogService _logService =
            ServiceExtension.Services.GetRequiredService<ILogService>();

        /// <summary>
        /// CancellationTokenSource
        /// </summary>
        private static readonly DependencyProperty CancellationTokenSourceProperty =
            DependencyProperty.RegisterAttached(
                "CancellationTokenSource",
                typeof(CancellationTokenSource),
                typeof(PersonPictureCacheHelper),
                new PropertyMetadata(null)
            );

        public static CancellationTokenSource GetCancellationTokenSource(DependencyObject obj) =>
            (CancellationTokenSource)obj.GetValue(CancellationTokenSourceProperty);

        public static void SetCancellationTokenSource(
            DependencyObject obj,
            CancellationTokenSource value
        ) => obj.SetValue(CancellationTokenSourceProperty, value);

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

            try
            {
                var oldCancellationTokenSource = GetCancellationTokenSource(obj);
                oldCancellationTokenSource?.Cancel();
                oldCancellationTokenSource?.Dispose();

                var newCancellationTokenSource = new CancellationTokenSource();
                obj.SetValue(CancellationTokenSourceProperty, new CancellationTokenSource());

                var personPictureUri = await _fileCache.GetFileUriAsync(
                    new Uri(source),
                    newCancellationTokenSource.Token
                );
                personPicture.ProfilePicture = new BitmapImage(personPictureUri)
                {
                    DecodePixelHeight = personPicture.MaxHeight is double.PositiveInfinity
                        ? 0
                        : (int)personPicture.MaxHeight,
                };
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                _logService.Error($"Image source ({source}) is invalid", ex);
            }
        }
    }
}
