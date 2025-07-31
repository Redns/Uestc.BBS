using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Uestc.BBS.Mvvm.Models;

namespace Uestc.BBS.WinUI.Controls
{
    public partial class CustomFlipView : FlipView
    {
        public CustomFlipView()
        {
            RenderTransform = new TranslateTransform();

            SelectionChanged += (_, e) =>
            {
#if DEBUG
                Debug.WriteLine(
                    $"SelectionChanged: {((BoardTabItemModel?)e.AddedItems.FirstOrDefault())?.Name}, {((BoardTabItemModel?)e.RemovedItems.FirstOrDefault())?.Name}"
                );
#endif
                if (e.AddedItems.Count == 0 || e.RemovedItems.Count == 0) return;

                int added = Items.IndexOf(e.AddedItems[0]);
                int removed = Items.IndexOf(e.RemovedItems[0]);
                if (added == removed) return;

                bool toRight = added > removed;
                double delta = ActualWidth;

                var transform = (TranslateTransform)RenderTransform;

                // 1. 滑出旧页
                var slideOut = new DoubleAnimation
                {
                    From = 0,
                    To = toRight ? -delta : delta,
                    Duration = TimeSpan.FromMilliseconds(300),
                    EasingFunction = new QuadraticEase()
                };
                var sbOut = new Storyboard();
                Storyboard.SetTarget(slideOut, transform);
                Storyboard.SetTargetProperty(slideOut, "X");
                sbOut.Children.Add(slideOut);

                
                // await sbOut.CompletedAsync();   // 等待结束

                // 2. 立即把偏移设到另一侧（无动画）
                transform.X = toRight ? delta : -delta;

                // 3. 滑入新页
                var slideIn = new DoubleAnimation
                {
                    From = toRight ? delta : -delta,
                    To = 0,
                    Duration = TimeSpan.FromMilliseconds(300),
                    EasingFunction = new QuadraticEase()
                };
                var sbIn = new Storyboard();
                Storyboard.SetTarget(slideIn, transform);
                Storyboard.SetTargetProperty(slideIn, "X");
                sbIn.Children.Add(slideIn);
                //sbOut.Begin();
                sbIn.Begin();
            };
        }
    }
}
