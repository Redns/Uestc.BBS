using System;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using WinUIEx;

namespace Uestc.BBS.WinUI.Controls
{
    public sealed partial class TopicOverview : UserControl
    {
        public string Avatar { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.Today;

        public bool IsHot { get; set; } = false;

        public string BoardName { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

        public string PreviewSource { get; set; } = string.Empty;

        public uint Views { get; set; }

        public uint Replies { get; set; }

        public uint Likes { get; set; }

        public TopicOverview()
        {
            InitializeComponent();
        }

        private void Image_PointerPressed(
            object sender,
            Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e
        )
        {
            var imageScrollViewer = new ScrollViewer
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                ZoomMode = ZoomMode.Enabled,
                Content = new Image()
                {
                    Source = new BitmapImage(new Uri(PreviewSource)),
                    Stretch = Stretch.Uniform,
                    MaxWidth = 1100,
                    MaxHeight = 700,
                },
            };

            // Êó±ê¹öÂÖËõ·Å
            //imageScrollViewer.PointerWheelChanged += (sender, args) =>
            //{
            //    if (sender is not ScrollViewer scrollViewer)
            //    {
            //        return;
            //    }

            //    var delta = args.GetCurrentPoint(scrollViewer).Properties.MouseWheelDelta;
            //    var zoomFactor =
            //        delta > 0 ? scrollViewer.ZoomFactor * 1.25f : scrollViewer.ZoomFactor / 1.25f;
            //    scrollViewer.ChangeView(
            //        null,
            //        null,
            //        Math.Max(
            //            scrollViewer.MinZoomFactor,
            //            Math.Min(zoomFactor, scrollViewer.MaxZoomFactor)
            //        )
            //    );

            //    args.Handled = true;
            //};

            var window = new WindowEx
            {
                Content = imageScrollViewer,
                ExtendsContentIntoTitleBar = true,
                SystemBackdrop = new MicaBackdrop(),
                PresenterKind = AppWindowPresenterKind.CompactOverlay,
            };

            window.SetWindowSize(1200, 800);
            window.Activate();
            window.CenterOnScreen();
        }
    }
}
