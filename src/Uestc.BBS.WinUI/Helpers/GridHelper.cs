using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Uestc.BBS.WinUI.Helpers
{
    public static class GridHelper
    {
        public static void SetRows(this Grid grid, int rows)
        {
            if (rows <= 1)
            {
                return;
            }

            for (int i = 0; i < rows; i++)
            {
                grid.RowDefinitions.Add(
                    new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) }
                );
            }
        }

        public static void SetColumns(this Grid grid, int columns)
        {
            if (columns <= 1)
            {
                return;
            }

            for (int i = 0; i < columns; i++)
            {
                grid.ColumnDefinitions.Add(
                    new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) }
                );
            }
        }

        public static void SetRowsAndColumns(this Grid grid, int rows, int columns)
        {
            SetRows(grid, rows);
            SetColumns(grid, columns);
        }

        /// <summary>
        /// 向 Grid 中添加元素
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="element"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public static void Add(
            this Grid grid,
            FrameworkElement element,
            int row = 0,
            int column = 0
        )
        {
            Grid.SetRow(element, row);
            Grid.SetColumn(element, column);
            grid.Children.Add(element);
        }

        public static void Add(
            this Grid grid,
            FrameworkElement element,
            int row = 0,
            int column = 0,
            int rowSpan = 1,
            int columnSpan = 1
        )
        {
            Grid.SetRow(element, row);
            Grid.SetColumn(element, column);

            if (rowSpan > 1)
            {
                Grid.SetRowSpan(element, rowSpan);
            }

            if (columnSpan > 1)
            {
                Grid.SetColumnSpan(element, columnSpan);
            }

            grid.Children.Add(element);
        }
    }
}
