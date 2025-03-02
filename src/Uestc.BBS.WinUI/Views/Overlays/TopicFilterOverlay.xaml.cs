using System;
using System.Collections.ObjectModel;
using CommunityToolkit.WinUI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.ApplicationModel.DataTransfer;

namespace Uestc.BBS.WinUI.Views.Overlays
{
    public sealed partial class TopicFilterOverlay : Page
    {
        public TopicFilterOverlay()
        {
            InitializeComponent();
            items =
            [
                "Item 1",
                "Item 2",
                "Item 3",
                "Item 4",
                "Item 5",
                "Item 6",
                "Item 7",
                "Item 8",
                "Item 9",
                "Item 10",
                "Item 11",
                "Item 12",
                "Item 13",
                "Item 14",
                "Item 15",
                "Item 16",
            ];
            MyListView.ItemsSource = items;
        }

        private ObservableCollection<string> items;
        private string draggedItem;
        private int originalIndex;
        private int placeholderIndex = -1;

        private void MyListView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            draggedItem = e.Items[0].ToString();
            originalIndex = items.IndexOf(draggedItem);
            e.Data.SetText(draggedItem);
            e.Data.RequestedOperation = DataPackageOperation.Move;
            items.RemoveAt(originalIndex);
        }

        private void MyListView_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Move;

            var listView = sender as ListView;
            var point = e.GetPosition(listView.ItemsPanelRoot);
            int targetIndex = -1;

            for (int i = 0; i < listView.Items.Count; i++)
            {
                var item = (ListViewItem)listView.ContainerFromIndex(i);
                if (item != null && point.Y < item.TransformToVisual(listView.ItemsPanelRoot).TransformPoint(new Windows.Foundation.Point(0, 0)).Y + item.ActualHeight / 2)
                {
                    targetIndex = i;
                    break;
                }
            }

            if (targetIndex == -1)
            {
                targetIndex = items.Count;
            }

            if (placeholderIndex != targetIndex)
            {
                if (placeholderIndex != -1 && placeholderIndex < items.Count)
                {
                    items.RemoveAt(placeholderIndex);
                }

                items.Insert(targetIndex, string.Empty);
                placeholderIndex = targetIndex;
            }
        }

        private void MyListView_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.Text))
            {
                string text = e.DataView.GetTextAsync().AsTask().Result;
                items[placeholderIndex] = text;
            }
        }

        private void MyListView_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
            if (args.DropResult == DataPackageOperation.None)
            {
                // If the drag operation was not completed successfully, put the item back to its original position
                items.RemoveAt(placeholderIndex);
                items.Insert(originalIndex, draggedItem);
            }

            // Reset the draggedItem, originalIndex, and placeholderIndex
            draggedItem = null;
            originalIndex = -1;
            placeholderIndex = -1;
        }
    }
}
