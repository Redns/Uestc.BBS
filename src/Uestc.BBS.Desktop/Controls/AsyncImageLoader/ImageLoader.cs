using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Uestc.BBS.Desktop.Controls.AsyncImageLoader.Loaders;

namespace Uestc.BBS.Desktop.Controls.AsyncImageLoader;

public static class ImageLoader
{
    public static readonly AttachedProperty<string?> SourceProperty =
        AvaloniaProperty.RegisterAttached<Image, string?>("Source", typeof(ImageLoader));

    public static readonly AttachedProperty<bool> IsLoadingProperty =
        AvaloniaProperty.RegisterAttached<Image, bool>("IsLoading", typeof(ImageLoader));

    static ImageLoader()
    {
        SourceProperty.Changed.AddClassHandler<Image>(OnSourceChanged);
    }

    /// <summary>
    /// 图片加载模式
    /// </summary>
    public static IAsyncImageLoader AsyncImageLoader { get; set; } = new DiskCachedWebImageLoader();

    private static readonly ConcurrentDictionary<Image, CancellationTokenSource> PendingOperations =
        new();

    private static async void OnSourceChanged(Image sender, AvaloniaPropertyChangedEventArgs args)
    {
        var url = args.GetNewValue<string?>();

        // Cancel/Add new pending operation
        var cts = PendingOperations.AddOrUpdate(
            sender,
            new CancellationTokenSource(),
            (image, cancellationTokenSource) =>
            {
                cancellationTokenSource.Cancel();
                return new CancellationTokenSource();
            }
        );

        if (string.IsNullOrEmpty(url))
        {
            PendingOperations.Remove(sender, out cts);
            sender.Source = null;
            return;
        }

        SetIsLoading(sender, true);

        await Task.Run(async () =>
        {
            // A small delay allows to cancel early if the image goes out of screen too fast (eg. scrolling)
            // The Bitmap constructor is expensive and cannot be cancelled
            await Task.Delay(10, cts.Token);

            try
            {
                var bitmap = await AsyncImageLoader.ProvideImageAsync(url);
                if (bitmap != null && !cts.Token.IsCancellationRequested)
                {
                    sender.Source = bitmap;
                }
            }
            catch { }
            finally
            {
                // "It is not guaranteed to be thread safe by ICollection, but ConcurrentDictionary's implementation is.
                // Additionally, we recently exposed this API for .NET 5 as a public ConcurrentDictionary.TryRemove"
                PendingOperations.Remove(sender, out cts);
                //(
                //    (ICollection<KeyValuePair<Image, CancellationTokenSource>>)PendingOperations
                //).Remove(new KeyValuePair<Image, CancellationTokenSource>(sender, cts));
                SetIsLoading(sender, false);
            }
        });
    }

    public static string? GetSource(Image element) => element.GetValue(SourceProperty);

    public static void SetSource(Image element, string? value) =>
        element.SetValue(SourceProperty, value);

    public static bool GetIsLoading(Image element) => element.GetValue(IsLoadingProperty);

    private static void SetIsLoading(Image element, bool value) =>
        element.SetValue(IsLoadingProperty, value);
}
