using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Uestc.BBS.Mvvm.Models;

namespace Uestc.BBS.WinUI.Controls
{
    public partial class CustomFlipView : FlipView
    {
        public CustomFlipView()
        {
            SelectionChanged += (_, e) =>
            {
                Debug.WriteLine(
                    $"SelectionChanged: {((BoardTabItemModel?)e.AddedItems.FirstOrDefault())?.Name}, {((BoardTabItemModel?)e.RemovedItems.FirstOrDefault())?.Name}"
                );

                if (e.AddedItems.Count > 0 && e.RemovedItems.Count > 0)
                {
                    var addedItemIndex = Items.IndexOf(e.AddedItems[0]);
                    var removedItemIndex = Items.IndexOf(e.RemovedItems[0]);

                    if (addedItemIndex != removedItemIndex)
                    {
                        var direction = addedItemIndex > removedItemIndex ? "Right" : "Left";
                        var storyboard = new Storyboard();

                        var slideOutAnimation = new DoubleAnimation
                        {
                            From = 0,
                            To = direction == "Right" ? -ActualWidth : ActualWidth,
                            Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                            EasingFunction = new QuadraticEase()
                        };
                        var slideInAnimation = new DoubleAnimation
                        {
                            From = direction == "Right" ? ActualWidth : -ActualWidth,
                            To = 0,
                            Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                            EasingFunction = new QuadraticEase()
                        };

                        Storyboard.SetTarget(slideOutAnimation, this);
                        Storyboard.SetTargetProperty(
                            slideOutAnimation,
                            "(UIElement.RenderTransform).(TranslateTransform.X)"
                        );
                        Storyboard.SetTarget(slideInAnimation, this);
                        Storyboard.SetTargetProperty(
                            slideInAnimation,
                            "(UIElement.RenderTransform).(TranslateTransform.X)"
                        );

                        storyboard.Children.Add(slideOutAnimation);
                        storyboard.Children.Add(slideInAnimation);

                        storyboard.Begin();
                    }
                }
            };
        }
    }
}
