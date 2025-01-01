﻿using System;
using Avalonia;
using Avalonia.Logging;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Uestc.BBS.Desktop.Controls.AsyncImageLoader.Loaders;

namespace Uestc.BBS.Desktop.Controls.AsyncImageLoader
{
    public static class ImageBrushLoader
    {
        public static IAsyncImageLoader AsyncImageLoader { get; set; } =
            new RamCachedWebImageLoader();

        static ImageBrushLoader()
        {
            SourceProperty.Changed.AddClassHandler<ImageBrush>(OnSourceChanged);
        }

        private static async void OnSourceChanged(
            ImageBrush imageBrush,
            AvaloniaPropertyChangedEventArgs args
        )
        {
            var (oldValue, newValue) = args.GetOldAndNewValue<string?>();
            if (oldValue == newValue)
                return;

            SetIsLoading(imageBrush, true);

            Bitmap? bitmap = null;
            try
            {
                if (newValue is not null)
                {
                    bitmap = await AsyncImageLoader.ProvideImageAsync(newValue);
                }
            }
            catch { }

            if (GetSource(imageBrush) != newValue)
                return;
            imageBrush.Source = bitmap;

            SetIsLoading(imageBrush, false);
        }

        public static readonly AttachedProperty<string?> SourceProperty =
            AvaloniaProperty.RegisterAttached<ImageBrush, string?>("Source", typeof(ImageLoader));

        public static string? GetSource(ImageBrush element)
        {
            return element.GetValue(SourceProperty);
        }

        public static void SetSource(ImageBrush element, string? value)
        {
            element.SetValue(SourceProperty, value);
        }

        public static readonly AttachedProperty<bool> IsLoadingProperty =
            AvaloniaProperty.RegisterAttached<ImageBrush, bool>("IsLoading", typeof(ImageLoader));

        public static bool GetIsLoading(ImageBrush element)
        {
            return element.GetValue(IsLoadingProperty);
        }

        private static void SetIsLoading(ImageBrush element, bool value)
        {
            element.SetValue(IsLoadingProperty, value);
        }
    }
}
